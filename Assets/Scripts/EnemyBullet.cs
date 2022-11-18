using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(this, 5f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //LOGICA DE DANO
            Destroy(this.gameObject);
            return;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
            return;
        }
    }
}
