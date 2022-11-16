using System;
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
    }

    // Update is called once per frame
    void Update()
    {
        float _angle = (180/math.PI) * math.atan2(agent.velocity.y,agent.velocity.x);

        bool canSeePlayer = FindPlayer();
        
        if(_spinCondition != SpinCondition.Spinning)
            this.transform.rotation = Quaternion.Euler(0, 0, (_state == EnemyState.Chase) ? (_playerDirectionAngle) : (_angle));
        
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
        //TODO ir pro lugar e dar uma rodadinha
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
    
    private void EnterInvestigate(Vector3 position)
    {
        _state = EnemyState.Investigate;
        SetDestination(position);
    }

    private void EnterChase()
    {
        _state = EnemyState.Chase;
        Debug.Log(this.name + " entrou estado de perseguição, cuidado!!!");
        statusIndicator.SetActive(true); //Setar cabeça vermelha
        
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
        
        Debug.Log(_playerDirectionAngle.ToString() + ' ' + (this.transform.localRotation.eulerAngles.z).ToString() + " " + math.abs(Mathf.DeltaAngle(_playerDirectionAngle,this.transform.localRotation.eulerAngles.z)));
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

        if (dist > noise.Strength && 
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
    
}