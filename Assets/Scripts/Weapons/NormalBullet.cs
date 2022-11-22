using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
    public int damage;
    private void Start()
    {
        Destroy(this,10f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        NoiseSystem.MakeNoise(transform.position, 4f,Noise.SoundType.BulletHit);

        GameObject obj = col.gameObject;

        if (obj.CompareTag("Player"))
        {
            //TODO logica de dano no jogador   
        }
        else if (obj.CompareTag("Enemy")){
            obj.GetComponent<Enemy>().takeDamage(damage);
        }

        Destroy(this.gameObject);
    }

}
