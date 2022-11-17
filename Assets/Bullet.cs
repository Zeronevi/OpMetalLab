using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float damage;
    private void Start()
    {
        Destroy(this,10f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
            NoiseSystem.MakeNoise(transform.position,4f);
            Destroy(this);
        
        Destroy(this.gameObject);
    }

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;
        
    }

    public float GetDamage()
    {
        return this.damage;
    }
}
