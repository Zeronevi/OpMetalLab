using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Target : Target
{
    private float DEFAULT_RADIUS = 1f;
    private int DEFAULT_STEPS = 20;

    private int steps;
    private float radius;

    private LineRenderer circleRender;

    private float K_CONTROL_OSCILATION = 1f;
    [SerializeField] private Component referenceToOscilation;
    private Model2 radialVariation, tangentialVariation, radiusVariation;
    

    protected override void Start()
    {
        circleRender = GetComponent<LineRenderer>();

        radius = DEFAULT_RADIUS;
        steps = DEFAULT_STEPS;

        float m = 0.5f;
        float k = 1f;
        float b = 0.0001f;

        radialVariation = new Model2(m, k, b, 0, 0);
        tangentialVariation = new Model2(m, k, b, 0, 0);
        radiusVariation = new Model2(m, k, b, 0, 0);
    }

    protected override void Draw()
    {
        circleRender.positionCount = steps;
        Vector2 currentPosition = transform.position;
        for (int currentStep = 0; currentStep < steps; currentStep++) 
        {
            float circunferenceProgress = ((float) currentStep) / ((float) steps)*2*Mathf.PI;

            Vector2 positionScaled = Vector2.zero;
            positionScaled.x = Mathf.Cos(circunferenceProgress);
            positionScaled.y = Mathf.Sin(circunferenceProgress);

            Vector2 positionStep = currentPosition + radius * positionScaled;

            circleRender.SetPosition(currentStep, positionStep);
        }
    }

    private void FixedUpdate()
    {
        radialVariation.ProportionalControl(Time.deltaTime, K_CONTROL_OSCILATION);
        tangentialVariation.ProportionalControl(Time.deltaTime, K_CONTROL_OSCILATION);
        radiusVariation.ProportionalControl(Time.deltaTime, K_CONTROL_OSCILATION);
    }

    protected override void UpdateOscilation()
    {
        Vector2 current_position = transform.position;
        if (referenceToOscilation == null)
        {
            transform.position = current_position;
            radius = DEFAULT_RADIUS;
        } 
        else
        {
            Vector2 radialDireciton = (Utils.GetMouseWorldPosition() - referenceToOscilation.transform.position).normalized;
            float angle = Utils.GetAngleFormVectorFloat(radialDireciton)*Mathf.PI/180;
            
            Vector2 oscilation = new Vector2(radialVariation.GetCurrentValue() * Mathf.Cos(angle) - tangentialVariation.GetCurrentValue() * Mathf.Sin(angle),
                                             radialVariation.GetCurrentValue() * Mathf.Sin(angle) + tangentialVariation.GetCurrentValue() * Mathf.Cos(angle));
            transform.position = current_position + oscilation;

            radius = DEFAULT_RADIUS + radiusVariation.GetCurrentValue();
        }
    }
}
