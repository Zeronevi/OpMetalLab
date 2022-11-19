using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
    
    private void Start()
    {
        Destroy(this,10f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        NoiseSystem.MakeNoise(transform.position, 4f);
        Destroy(this);

        Destroy(this.gameObject);
    }

}
