using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model2
{
    //Modelo de segunda ordem (Mola, Amortecedor, Massa) com controle PID
    private int DEFAULT_DATA_HIST_LEN = 10;
    private float wn;
    private float zeta;

    private float reference_value;
    private float current_value;
    private float current_speed;

    private List<float> valuesToIntegration = null;
    int selectedValueIndex = 0;

    public Model2(float wn, float zeta, float initial_value, float initial_speed)
    {
        this.wn = wn;
        this.zeta = zeta;

        this.reference_value = initial_value;
        this.current_value = initial_value;
        
        this.current_speed = initial_speed;

        valuesToIntegration = new List<float>();
        for(int index = 0; index < DEFAULT_DATA_HIST_LEN; index++)
        {
            valuesToIntegration.Add(0);
        }

    }

    private float lastValue = 0;
    public void ProportionalControl(float deltaT, float kp_control, float ki_control, float kd_control)
    {
        float error = this.reference_value - this.current_value;

        valuesToIntegration[selectedValueIndex % DEFAULT_DATA_HIST_LEN] = error;

        float dError = (error - lastValue) / deltaT;

        lastValue = error;
        selectedValueIndex++;
        if (selectedValueIndex > DEFAULT_DATA_HIST_LEN) selectedValueIndex -= DEFAULT_DATA_HIST_LEN;

        float integrateError = 0;
        foreach (float errorValue in valuesToIntegration) integrateError += errorValue;
        
        float force = (kp_control * error) + (ki_control*integrateError) + (kd_control* dError);
        Vector2 newState = Utils.eulerOrdem2(deltaT, this.current_value, this.current_speed, force, this.wn, this.zeta);
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
