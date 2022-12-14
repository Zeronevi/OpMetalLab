using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour
{

    public static float NORMAL_SPEED = 6f;
    public static float RUN_SPEED = 10f;
    public WeaponInventory wp;

    private float speed;
    Vector2 _velocity;
    private Rigidbody2D _rb;
    private bool _canMove = true;

    private void Awake()
    {
        if (PlayerStatus.GetInstance() != null) PlayerStatus.GetInstance();
        Enemy.ResetListEnemy();
    }

    void Start()
    {
        _velocity = new Vector2();
        _rb = GetComponent<Rigidbody2D>();
        speed = NORMAL_SPEED;
        PlayerStatus.GetInstance().ResetStatus();
    }

    void Update()
    {
        LookAtMouse();
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

    public void SetSpeed(float value)
    {
        speed = value;
    }
    
}
