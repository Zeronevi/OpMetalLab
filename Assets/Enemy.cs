using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject _gameObject;
    private NavMeshAgent _agent;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_gameObject.transform.position);
        float _angle = (180/math.PI) * math.atan2(_agent.velocity.y,_agent.velocity.x);
        this.transform.rotation = Quaternion.Euler(0, 0,_angle);
    }
}
