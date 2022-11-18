using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour
{
    public float speed;
    Vector2 _velocity;
    private Rigidbody2D _rb;
    private bool _canMove = true;

    //Depois refatorar em nova classe1
    public GameObject bullet,gunBarrel;
    public float bulletSpeed;
    
    void Start()
    {
        _velocity = new Vector2();
        _rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        LookAtMouse();
        if(Input.GetKeyDown(KeyCode.Mouse0)) Shoot();
        
        Move();
    }

    void LookAtMouse()
    {
        float angle = (180/math.PI) * math.atan2(   SharedContent.MousePosition.y - transform.position.y, 
                                                    SharedContent.MousePosition.x - transform.position.x);
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Move()
    {
        _velocity.x = Input.GetAxisRaw("Horizontal");
        _velocity.y = Input.GetAxisRaw("Vertical");
        _velocity = _velocity.normalized;        

        if (_canMove) _rb.velocity = _velocity*speed;
    }
    
    void Shoot()
    {
        var objBullet = Instantiate(this.bullet, gunBarrel.transform.position,bullet.transform.rotation);
        objBullet.GetComponent<Rigidbody2D>().velocity = 
            bulletSpeed*(SharedContent.MousePosition - (Vector2)transform.position).normalized;
        
        NoiseSystem.MakeNoise(transform.position,10f,Noise.SoundType.Gunshot);
    }
}
