using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float damage;
    public int damage = 10;
    public GameObject bloodEffect;
    public PlayerStatus ps;
    
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

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;
        
    }

    public float GetDamage()
    {
        return this.damage;

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {

        Enemy enemy = hitInfo.GetComponent<Enemy>();
        Debug.Log(hitInfo.name);
        if (enemy != null)
        {
            //ps.current_life = ps.current_life - 1000;

                     
            enemy.takeDamage(10);
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, -0.5f);
            Destroy(this.gameObject);
            var bloodE = Instantiate(bloodEffect, pos, transform.rotation);
            Destroy(bloodE, 0.7f);

        }

    }
}
