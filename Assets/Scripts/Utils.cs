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

    public static float euler(float deltaT, float x, float force, float k, float b)
    {
        float dydt = (force - k * x) / b;
        float newX = x + deltaT * dydt;
        return newX;
    }

    public static Vector2 euler(float deltaT, Vector2 x, Vector2 force, float k, float b)
    {
        Vector2 dydt= (force - k * x)* (1f/b);
        Vector2 newX = x + deltaT * dydt;
        return newX;
    }

}
