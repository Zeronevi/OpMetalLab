using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 moveTarget;
    public static List<GameObject> enemyList;
    public GameObject bloodEffect2;

    public int health = 50;
    public NavMeshAgent _agent;
    
    void Start()
    {
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
        Destroy(this.gameObject);
    }
    public void takeDamage(int damage)
    {
        Debug.Log(damage);
        health = health - damage;

        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        if (health <= 0)
        {
            Instantiate(bloodEffect2, this.transform.position, transform.rotation);
            Die();
        }
    }
}
