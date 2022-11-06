using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils :MonoBehaviour
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
    
    public static float euler(float deltaT, float x0, float dxdt)
    {
        float x = x0 + dxdt * deltaT;
        return x;
    }

    public static float eulerOrdem1(float deltaT, float x, float force, float k, float b)
    {
        float dxdt = (force - k * x) / b;
        float newX = euler(deltaT, x, dxdt);
        return newX;
    }

    public static Vector2 eulerOrdem2(float deltaT, float position,float speed, float force,float m, float k, float b)
    {
        float aceleration = -(b/m)*speed - (k/m) * position + force/m;

        float newPosition = position + speed * deltaT + (aceleration / 2) * (deltaT*deltaT);
        float newSpeed = speed + aceleration * deltaT;

        Vector2 result = Vector2.zero;
        result.x = newPosition;
        result.y = newSpeed;
        return result;
    }

}
