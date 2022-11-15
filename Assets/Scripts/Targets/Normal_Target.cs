using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_Target : Target
{
    private int DEFAULT_STEPS = 20;

    private int steps;

    private LineRenderer circleRender;

    private float KP_CONTROL;
    private float KI_CONTROL;
    private float KD_CONTROL;

    private Model1 radius;
    private Model2 radiusVariation;
    

    protected override void Start()
    {
        circleRender = GetComponent<LineRenderer>();

        steps = DEFAULT_STEPS;

        KP_CONTROL = DEFAULT_KP_CONTROL;
        KI_CONTROL = DEFAULT_KI_CONTROL;
        KD_CONTROL = DEFAULT_KD_CONTROL;

        float wn = 1f;
        float zeta = 0.1f;

        radius = new Model1(0.01f, 0.01f, DEFAULT_RADIUS);
        radiusVariation = new Model2(wn, zeta, 0, 0);
    }

    protected override void UpdatePosition()
    {
        Vector2 MouseWorldPosition = SharedContent.MousePosition;
        Vector2 CameraPosition = Vector2.zero;

        Vector2 MouseScreenPosition = MouseWorldPosition - CameraPosition;
        transform.position = MouseScreenPosition;
    }

    protected override void Draw()
    {
        circleRender.positionCount = steps;
        Vector2 currentPosition = transform.position;

        float radius = Mathf.Abs(this.radius.GetCurrentValue() + radiusVariation.GetCurrentValue());
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

    public override Vector2 GetPositionTarget()
    {
        return GetRandomPositionInTarget();
    }

    public override void setCorrectRadius(float radius)
    {
        this.radius.SetReferenceValue(radius);
    }

    public override void setControl(float kp, float ki, float kd)
    {
        KD_CONTROL = kd;
        KI_CONTROL = ki;
        KP_CONTROL = kp;
    }

    public Vector2 GetRandomPositionInTarget()
    {
        float randomRadius = Random.Range(0f, radius.GetCurrentValue());
        float angle = Random.Range(0f, 2 * Mathf.PI);

        Vector2 result = transform.position;
        result.x += randomRadius * Mathf.Cos(angle);
        result.y += randomRadius * Mathf.Sin(angle);
        return result;
    }

    protected override void Control(float time)
    {
        radius.ProportionalControl(time, 0.5f);
        radiusVariation.ProportionalControl(time, KP_CONTROL, KI_CONTROL, KD_CONTROL);
    }


    private Vector2 lastPostion;
    protected override void UpdateOscilation(float time)
    {
        Vector2 currentPosition = transform.position;
        if (lastPostion == null)
        {
            lastPostion = currentPosition;
            return;
        }

        Vector2 diff = currentPosition - lastPostion;
        float magnitudeDiff = diff.magnitude;

        float fator = 1f;

        float referenceValue = fator * (magnitudeDiff);

        radiusVariation.SetReferenceValue(referenceValue);
        lastPostion = currentPosition;
    }
}
