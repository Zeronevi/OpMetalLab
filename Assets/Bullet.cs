using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(this,5f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            NoiseSystem.MakeNoise(transform.position,2f);
            Destroy(this);
        }
        
        Destroy(this.gameObject);
    }
}
