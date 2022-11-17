using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moviment : MonoBehaviour
{

    [SerializeField] private float angular_speed = 1f;
    [SerializeField] private float maxVariationAngle = 20f;
    [SerializeField] private float time_wait = 2f;

    private float current_angle;
    private float current_speed;
    private float time = 0;

    private float initial_angle;
    // Start is called before the first frame update
    void Start()
    {
        current_angle = -maxVariationAngle;
        current_speed = angular_speed;
        initial_angle = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time < 0) time = 0;

            if (time > 0) return;
        }

        float deltaAngle = current_speed * Time.deltaTime;
        float newAngle = current_angle + deltaAngle;
        if (newAngle >= maxVariationAngle)
        {
            newAngle = maxVariationAngle;
            current_speed = -angular_speed;
            time = time_wait;
        }
        else if (newAngle <= -maxVariationAngle)
        {
            newAngle = -maxVariationAngle;
            current_speed = angular_speed;
            time = time_wait;
        }
        current_angle = newAngle;
        transform.rotation = Quaternion.Euler(0, 0, current_angle+initial_angle);
    }

}
