using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> enemyList;
    public static void ResetListEnemy()
    {
        if(enemyList == null)
            enemyList = new List<Enemy>();

        enemyList.Clear();
    }


    [SerializeField] LifeBar life_bar = null;
    [SerializeField] GameObject MASTER_OBJ = null;

    public GameObject bloodEffect2;
    public GameObject bloodEffect1;

    //--Controlador dos estados
    public enum EnemyState
    {Idle, Patrol, Investigate, Chase};
    [SerializeField] private EnemyState startingState;
    private EnemyState _state;

    //--Patrulha
    public List<GameObject> patrolPoints;
    private int _patrolPointIndex;
    private const float Epsilon = 0.3f;
    private const float InvEpsilon = 1.5f;

    private enum SpinCondition
    { NotSpun, Spinning, Spun }
    private SpinCondition _spinCondition = SpinCondition.NotSpun;
    private const float SpinDelay = .25f;
    private Coroutine _spinning;
    
    //--Visao
    public LayerMask mask;
    public GameObject player;
    public float fov;
    private Vector2 _playerDirection;
    private float _playerDirectionAngle;
        
    //--Audicao--
    private Noise _investigationTarget = null;
    private const float SoundThreshold = 1;
    private NoiseSystem noiseSystem;

    //--PathFinding-- 
    public Vector2 moveTarget;
    public NavMeshAgent agent;

    //--Combate--
    public float MAX_HEALTH = 100;
    public float health = 100;

    [SerializeField] private float delay;
    private bool _canShoot = true;
    
    public GameObject gunBarrel;
    public GameObject bulletPrefab;
    [Range(0, 20)] public float bulletSpeed;
    [Range(0, 20)] public float bulletSpreadAngle;

    //recarregar arma??
    
    //Misc.
    //public GameObject statusIndicator;
    
    void Start()
    {
        noiseSystem = FindObjectOfType<NoiseSystem>();

        player = GameObject.Find("Player");
        health = MAX_HEALTH;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        jukebox = GetComponent<AudioSource>();
        
        if (enemyList == null) enemyList = new List<Enemy>();
        enemyList.Add(this.gameObject.GetComponent<Enemy>());

        _state = startingState;

        _investigationTarget = null;
        
        switch (_state)
        {
            case EnemyState.Idle: 
                EnterIdle();
                break;
            case EnemyState.Patrol:
                EnterPatrol();
                break;
            default:
                EnterIdle();
                break;
        }

        StartCoroutine(StepNoiseMaker());
    }

    // Update is called once per frame
    void Update()
    {
        float _angle = (180/math.PI) * math.atan2(agent.velocity.y,agent.velocity.x);

        bool canSeePlayer = FindPlayer();

        if (_state == EnemyState.Chase || _spinCondition != SpinCondition.Spinning && agent.velocity.magnitude > 0)
        {
            this.transform.rotation = Quaternion.Euler(-2, 0, (_state == EnemyState.Chase) ? (_playerDirectionAngle) : (_angle));
        }
        
        switch(_state)
        {
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Investigate:
                InvestigateUpdate();
                break;
            case EnemyState.Chase:
                ChaseUpdate(canSeePlayer);  
                break;
        }
    }


    private void IdleUpdate()
    {
        
    }

    private void PatrolUpdate()
    {
        if (_spinCondition == SpinCondition.Spinning) 
            return;
        
        if(Vector2.Distance(transform.position, patrolPoints[_patrolPointIndex].transform.position) < Epsilon)
        {
            if (_spinCondition == SpinCondition.NotSpun)
            {
               _spinning = StartCoroutine(Spin());
            }
            else if(_spinCondition == SpinCondition.Spun)
            {
                _spinning = null;
                _spinCondition = SpinCondition.NotSpun;
                //vai pro pr??ximo ponto de patrulha.
                _patrolPointIndex = (_patrolPointIndex + 1) % patrolPoints.Count;
                SetDestination(patrolPoints[_patrolPointIndex].transform.position);
            }
        }
    }

    private void InvestigateUpdate()
    {
        if(Vector2.Distance(transform.position, _investigationTarget.Position) < InvEpsilon)
        {
            if (_spinCondition == SpinCondition.NotSpun)
            {
                _spinning = StartCoroutine(Spin());
            }
            else if (_spinCondition == SpinCondition.Spun)
            {
                _spinning = null;
                _spinCondition = SpinCondition.NotSpun;
                _investigationTarget = null;
                //volta para o modo de patrulha.
                EnterPatrol();
                _investigationTarget = null;
            }
        } 
    }

    private void ChaseUpdate(bool seesPlayer)
    {
        if (_canShoot && seesPlayer)
        {
            Shoot();
        }

        agent.isStopped = seesPlayer && Vector2.Distance(transform.position, player.transform.position) < 4;
    }

    private void EnterIdle()
    {
        _state = EnemyState.Idle;
    }
    
    private void EnterPatrol()
    {
        _state = EnemyState.Patrol;
        if(patrolPoints?.Count  < 2) EnterIdle();
        SetDestination(patrolPoints[_patrolPointIndex].transform.position);
    }
    
    private void EnterInvestigate(Vector3 position)
    {
        _state = EnemyState.Investigate;
        if(_spinning != null)
            StopCoroutine(_spinning);
        _spinCondition = SpinCondition.NotSpun;
        SetDestination(position);
    }

    public void EnterChase()
    {
        if (_state == EnemyState.Chase) return;
           
        _state = EnemyState.Chase;
        Debug.Log(this.name + " entrou estado de persegui????o, cuidado!!!");
        //TODO colocar indicador de status
        //statusIndicator.SetActive(true); //Setar cabe??a vermelha
        
        if(_spinning != null)
            StopCoroutine(_spinning);
        StartCoroutine(ChaseRoutine());
        StartCoroutine(RateOfFireDelay());//evita o tiro instantaneo ao ser alertado
        List<string> sentences = new List<string>();
        sentences.Add("Hey, bro, come here NOW!");
        sentences.Add("I'll kill you!");
        sentences.Add("WHAT? How do you enter here?");
        sentences.Add("Oh! Why are you runnning?");
        
        Speak(sentences[random.Next(0, sentences.Count)]);
    }



    private Random random = new Random();
    //
    // --  Sentidos e Acoes
    //

    private void SetDestination(Vector2 position)
    {
        moveTarget = position;
        agent.SetDestination(moveTarget);
    }

    private bool FindPlayer()
    {
        _playerDirection = (player.transform.position - transform.position);
        _playerDirectionAngle = (180/math.PI) * math.atan2(_playerDirection.y,_playerDirection.x);
        
        RaycastHit2D hit = Physics2D.Raycast(gunBarrel.transform.position, _playerDirection,200f,layerMask:mask);
        
        Debug.DrawRay(this.transform.position,_playerDirection);
        
        if (hit.collider.gameObject.CompareTag("Player") && math.abs(Mathf.DeltaAngle(_playerDirectionAngle,transform.localRotation.eulerAngles.z)) < fov)
        {
            _spinCondition = SpinCondition.NotSpun;
            if (_state != EnemyState.Chase)
            {
                EnterChase();
            }
            
            return true;
        }

        return false;
    }
    
    public void Listen(Noise noise)
    {
        if (_state == EnemyState.Chase) return;
            
        float dist = Vector2.Distance(this.transform.position, noise.Position);

        print("Ouvi!" + noise.Strength + " "+ dist);
        if (dist < noise.Strength && 
            (_investigationTarget == null || _investigationTarget?.soundType < noise.soundType))
        {
            _state = EnemyState.Investigate;
            _investigationTarget = noise;
            EnterInvestigate(noise.Position);
        }
    }

    private AudioSource jukebox = null;
    private void Shoot()
    {
        if (_canShoot)
        {
            //TODO ADICIONAR SFX E VISUALIZA????O DO SOM
            var _bullet = Instantiate(bulletPrefab);
            _bullet.transform.position = gunBarrel.transform.position;
            
            float bulletAngle = Mathf.Deg2Rad*(_playerDirectionAngle +  UnityEngine.Random.Range(-bulletSpreadAngle,bulletSpreadAngle));
            Vector2 bulletDirection = new Vector2(math.cos(bulletAngle), math.sin(bulletAngle));
            _bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection*bulletSpeed;
            StartCoroutine(RateOfFireDelay());

            //noiseSystem.AddEnemyStep(this.transform.position, 15f);
            jukebox.Play();
        }
    }

    private IEnumerator RateOfFireDelay()
    {
        _canShoot = false;
        yield return new WaitForSeconds(delay);
        Debug.Log(name + " gun is now ready!");
        _canShoot = true;
    }

    private IEnumerator ChaseRoutine()
    {
        while(isActiveAndEnabled)
        {
            yield return new WaitForSeconds(1.5f);
            if(Vector2.Distance(transform.position,player.transform.position) > 7)
                SetDestination(player.transform.position);
        }
    }

    private IEnumerator Spin()
    {
        _spinCondition = SpinCondition.Spinning;
        for (int i = 0; i < 360; i += 1)
        {
            transform.Rotate(new Vector3(0,0,1));
            yield return new WaitForSeconds((float)1/120);
        }

        _spinCondition = SpinCondition.Spun;
    }

    private IEnumerator StepNoiseMaker()
    {
        while (isActiveAndEnabled)
        {
            if (agent.velocity.magnitude > 0.05f)
                if(noiseSystem != null) noiseSystem.AddEnemyStep(transform.position, 0.15f);
            yield return new WaitForSeconds(.30f);
        }
    }

    public void takeDamage(float damage)
    {
        Debug.Log(damage);
        health = health - damage;
        if (life_bar != null) life_bar.SetLife(health / MAX_HEALTH);

        var bloodE = Instantiate(bloodEffect1, gameObject.transform.position, transform.rotation);
        Destroy(bloodE, 0.7f);

        if (health <= 0)
        {
            Instantiate(bloodEffect2, this.transform.position, transform.rotation);
            Die();
        }
        else
        {
            EnterChase();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Enemy.enemyList.Remove(this.gameObject.GetComponent<Enemy>());
        Destroy(MASTER_OBJ);
    }

    [SerializeField] TextBox textBox = null;
    private void Speak(string text)
    {
        TextBox textbox = Instantiate(this.textBox, Vector2.zero, Quaternion.identity);
        textbox.SetMessage(text);
        textbox.SetReferenceObj(gameObject);
        textbox.Enable();
    }

}