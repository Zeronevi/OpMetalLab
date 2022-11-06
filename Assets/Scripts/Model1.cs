using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model1
{
    //Modelo de primeira ordem (Mola, Amortecedor, Sem Massa) com controle PID
    private float k;
    private float b;

    private float reference_value;
    private float current_value;

    public Model1(float k, float b, float initial_value)
    {
        this.k = k;
        this.b = b;
        this.reference_value = initial_value;
        this.current_value = initial_value;
    }

    public void ProportionalControl(float deltaT, float k_control)
    {
        float error = this.reference_value - this.current_value;
        float force = (k_control * error);
        this.current_value = Utils.eulerOrdem1(deltaT, this.current_value, force, this.k, this.b);
    }

    public void SetReferenceValue(float reference_value)
    {
        this.reference_value = reference_value;
    }

    public float GetCurrentValue()
    {
        return current_value;
    }

    public float GetReferenceValue()
    {
        return reference_value;
    }
}
