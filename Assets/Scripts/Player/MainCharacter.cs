using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MainCharacter : MonoBehaviour
{

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFormVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;

    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 MousePosition = Vector3.zero;
        MousePosition.x = SharedContent.MousePosition.x;
        MousePosition.y = SharedContent.MousePosition.y;

        return MousePosition;
    }



    public float speed;
    Vector2 _velocity;

    private Rigidbody2D _rb;
    private bool _canMove = true;
    [SerializeField] private FieldOfView fieldOfView;
    
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

        fieldOfView.UpdatePositionAndDirection(transform.position, GetAimDirection());
      
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
    
    void Shoot(){
        print("Pew!");
    }

    public Vector3 GetAimDirection()
    {
        Vector3 MousePositon = GetMouseWorldPosition();
        Vector3 CharacterPosition = transform.position;

        Vector3 aimDirection = (MousePositon - CharacterPosition).normalized;
        return aimDirection;
    }
}
