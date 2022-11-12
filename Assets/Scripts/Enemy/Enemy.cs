using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> enemyList;
    
    //--Controlador dos estados
    public enum EnemyState
    {Idle, Patrol, Investigate, Chase};
    [SerializeField] private EnemyState startingState;
    private EnemyState _state;

    //--Patrulha
    public List<GameObject> patrolPoints;
    private int _patrolPointIndex;
    private const float Epsilon = 0.3f;

    //--Visao
    public LayerMask mask;
    public GameObject player;
    public float fov;
    private Vector2 _playerDirection;
    private float _playerDirectionAngle;
        
    //--Audicao--
    private Noise _investigationTarget = null;
    private const float SoundThreshold = 1;

    //--PathFinding-- 
    public Vector2 moveTarget;
    public NavMeshAgent agent;
    
    //--Combate--
    [SerializeField] private int health, maxHealth;
    [SerializeField] private float delay;
    private bool _canShoot = true;
        //recarregar arma??
    
    //Misc.
    public GameObject statusIndicator;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (enemyList == null) enemyList = new List<Enemy>();
        enemyList.Add(this.gameObject.GetComponent<Enemy>());

        _state = startingState;
    }

    // Update is called once per frame
    void Update()
    {
        float _angle = (180/math.PI) * math.atan2(agent.velocity.y,agent.velocity.x);

        bool canSeePlayer = FindPlayer();
        
        this.transform.rotation =
            Quaternion.Euler(0, 0, (_state == EnemyState.Chase) ? (_playerDirectionAngle) : (_angle));
        
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
        //TODO>Fazer
        //if(flag da rodadinha == rodando)
        // return
        
        if(Vector2.Distance(transform.position, patrolPoints[_patrolPointIndex].transform.position) < Epsilon)
        {  
            //if(flag da rodadinha == nao rodou)
                //flag da rodadinha = rodando;
                //startcoroutine(rodadinha);
                //return
            
            //depois de ter rodado:
            _patrolPointIndex = (_patrolPointIndex + 1) % patrolPoints.Count;
            SetDestination(patrolPoints[_patrolPointIndex].transform.position);
        }
    }

    private void InvestigateUpdate()
    {
        //TODO>FAZER
        //Vai pro ponto de investigação
        //dah uma rodadinha
        
        //voltar para patrulha no caso da posicao do som nao ter nada, setar som da investigacao para nulo
    }

    private void ChaseUpdate(bool seesPlayer)
    {
        if (_canShoot && seesPlayer)
        {
            Shoot();
        }

        agent.isStopped = Vector2.Distance(transform.position, player.transform.position) < 7 ;
    }

    private void EnterIdle()
    {
        _state = EnemyState.Idle;
    }
    
    private void EnterPatrol()
    {
        _state = EnemyState.Patrol;
        if(patrolPoints?.Count  < 2) EnterIdle();
        
        SetDestination(patrolPoints[0].transform.position);
    }
    
    private void EnterInvestigate()
    {
        _state = EnemyState.Investigate;
        //--AHHH, ME ajudem ouvi algo
        //Setar cabeça amarela
    }

    private void EnterChase()
    {
        _state = EnemyState.Chase;
        Debug.Log(this.name + " entrou estado de perseguição, cuidado!!!");
        statusIndicator.SetActive(true); //Setar cabeça vermelha
        //Alertar guardas proximos
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position,_playerDirection,20f,layerMask:mask);
        
        if (hit.collider.gameObject.CompareTag("Player") && math.abs(_playerDirectionAngle-transform.rotation.z) < fov){
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
        float dist = Vector2.Distance(this.transform.position, noise.Position);
        float intensity = noise.Strength / (dist*dist);

        if (intensity >= SoundThreshold && 
            (_investigationTarget == null || _investigationTarget?.soundType < noise.soundType))
        {
            _state = EnemyState.Investigate;
            _investigationTarget = noise;
            SetDestination(noise.Position);
        }
    }

    private void Shoot()
    {
        if (_canShoot)
        { 
            Debug.Log("<Barulho de Tiro>");
            //var bullet = Instantiate(blabla);
            //bullet.transform.position = gunBarrelEnd;
            //float bulletAngle = angle + Random.range(-a,+a);
            //Vector2 bulletDirection = new Vector2(math.cos(bulletAngle{???? precisa passar pra rad???}, math.sin(bulletAngle){??? mesma cooisa}));
            //bullet.getComponent<RigidBody2D>().velocity = bulletDirection*bulletSpeed;
            
            //TODO Instanciar bala em direcao ao jogador
            //TODO Sistema de balas do inimigo, que ignore outros inimigos
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
}