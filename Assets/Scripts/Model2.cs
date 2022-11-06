using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model2
{
    //Modelo de segunda ordem (Mola, Amortecedor, Massa) com controle PID
    private float k;
    private float b;
    private float m;

    private float reference_value;
    private float current_value;
    private float current_speed;

    public Model2(float m, float k, float b, float initial_value, float initial_speed)
    {
        this.m = m;
        this.k = k;
        this.b = b;

        this.reference_value = initial_value;
        this.current_value = initial_value;
        
        this.current_speed = initial_speed;
    }

    public void ProportionalControl(float deltaT, float k_control)
    {
        float error = this.reference_value - this.current_value;
        float force = (k_control * error);
        Vector2 newState = Utils.eulerOrdem2(deltaT, this.current_value, this.current_speed, force, this.m, this.k, this.b);
        current_value = newState.x;
        current_speed = newState.y;
    }

    public void SetReferenceValue(float referenceValue)
    {
        this.reference_value = referenceValue;
    }

    public float GetCurrentValue()
    {
        return current_value;
    }

    public float GetCurrentSpeed()
    {
        return current_speed;
    }
}
