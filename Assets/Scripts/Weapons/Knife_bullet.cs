using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_bullet : Bullet
{
    public float angleSpeed = 60f;

    public float radius = 0.1f;
    public float initial_angle = 0;
    public float angle = 0;
    public float rangeAngle = 60f;

    public Vector2 center = Vector2.zero;

    private EdgeCollider2D edgeCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        
    }

    bool active = false;
    public void updateParameters(Vector2 center, float radius, float centerAngle, float angleRange)
    {
        
        this.center = center;
        this.radius = radius;
        this.initial_angle = centerAngle - angleRange / 2f;
        this.angle = initial_angle;
        this.rangeAngle = angleRange;
        active = true;
    }

    private void FixedUpdate()
    {
        if (!active) return;

        List<Vector2> points = new List<Vector2>();
        points.Add(Vector2.zero);
        float radianos = angle * Mathf.PI / 180f;
        points.Add(radius * new Vector2(Mathf.Cos(radianos), Mathf.Sin(radianos)));
        edgeCollider.SetPoints(points);

        angle += Time.fixedDeltaTime * angleSpeed;
        if (angle > initial_angle + rangeAngle) Destroy(gameObject);
    }
}
