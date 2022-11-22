using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
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
    [SerializeField] public NoiseSystem noiseSystem;

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
        player = GameObject.Find("Player");
        health = MAX_HEALTH;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        if (enemyList == null) enemyList = new List<Enemy>();
        enemyList.Add(this.gameObject.GetComponent<Enemy>());

        _state = startingState;

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
        
        if(_spinCondition != SpinCondition.Spinning)
            this.transform.rotation = Quaternion.Euler(0, 0, (_state == EnemyState.Chase) ? (_playerDirectionAngle) : (_angle));
        
        
        
        //TODO ADICIONAR SFX e VISUALIZAÇÃO DE "PASSOS NO ESCURO"
        //FORMATO: Corroutine
        
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
                //vai pro próximo ponto de patrulha.
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
            else if(_spinCondition == SpinCondition.Spun)
            {
                _spinning = null;
                _spinCondition = SpinCondition.NotSpun;
                _investigationTarget = null;
                //volta para o modo de patrulha.
                EnterPatrol();
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
        _state = EnemyState.Chase;
        Debug.Log(this.name + " entrou estado de perseguição, cuidado!!!");
        //TODO colocar indicador de status
        //statusIndicator.SetActive(true); //Setar cabeça vermelha
        
        if(_spinning != null)
            StopCoroutine(_spinning);
        StartCoroutine(ChaseRoutine());
    }
    
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
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _playerDirection,200f,layerMask:mask);
        
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

        if (dist < noise.Strength && 
            (_investigationTarget == null || _investigationTarget?.soundType < noise.soundType))
        {
            _state = EnemyState.Investigate;
            _investigationTarget = noise;
            EnterInvestigate(noise.Position);
        }
    }

    private void Shoot()
    {
        if (_canShoot)
        { 
            //TODO ADICIONAR SFX E VISUALIZAÇÃO DO SOM
            var _bullet = Instantiate(bulletPrefab);
            _bullet.transform.position = gunBarrel.transform.position;
            
            float bulletAngle = Mathf.Deg2Rad*(_playerDirectionAngle +  UnityEngine.Random.Range(-bulletSpreadAngle,bulletSpreadAngle));
            Vector2 bulletDirection = new Vector2(math.cos(bulletAngle), math.sin(bulletAngle));
            _bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection*bulletSpeed;
            StartCoroutine(RateOfFireDelay());
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
                noiseSystem.AddEnemyStep(transform.position, 0.15f);
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
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Enemy.enemyList.Remove(this.gameObject.GetComponent<Enemy>());
        Destroy(MASTER_OBJ);
    }

}