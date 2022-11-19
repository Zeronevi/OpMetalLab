using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
public class Enemy : MonoBehaviour
{
    public float MAX_HEALTH = 100;
    [SerializeField] LifeBar life_bar = null;
    [SerializeField] GameObject MASTER_OBJ = null;

    // Start is called before the first frame update
    public Vector2 moveTarget;
    public static List<GameObject> enemyList;
    public GameObject bloodEffect2;
    public GameObject bloodEffect1;

    public float health = 100;
    public NavMeshAgent _agent;
    
    void Start()
    {
        health = MAX_HEALTH;
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        moveTarget = this.transform.position;
        
        if (enemyList == null) enemyList = new List<GameObject>();
        enemyList.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        float _angle = (180/math.PI) * math.atan2(_agent.velocity.y,_agent.velocity.x);
        this.transform.rotation = Quaternion.Euler(0, 0,_angle);
    }

    public void SetDestiny(Vector2 position)
    {
        moveTarget = position;
        _agent.SetDestination(moveTarget);
    }


    private void Die()
    {
        gameObject.SetActive(false);
        Enemy.enemyList.Remove(this.gameObject);
        Destroy(MASTER_OBJ.gameObject);
    }
    public void takeDamage(int damage)
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
}
