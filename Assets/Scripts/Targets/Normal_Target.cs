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

    [SerializeField] private MainCharacter player = null;
    [SerializeField] private float AREA_LIMITE_RADIUS = 1.2f; 

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

        MAX_RADIUS = DEFAULT_MAX_RADIUS;
    }

    protected override void UpdatePosition()
    {
        Vector2 MouseScreenPosition;
        if (player != null)
        {
            Vector2 MouseWorldPosition = SharedContent.MousePosition;
            Vector2 playerPosition = player.transform.position;
            Vector2 diffMouseAndPlayer = MouseWorldPosition - playerPosition;

            float minRadius = AREA_LIMITE_RADIUS + Mathf.Abs(radius.GetCurrentValue() + radiusVariation.GetCurrentValue());
            if (diffMouseAndPlayer.magnitude < minRadius)
            {
                MouseScreenPosition = diffMouseAndPlayer.normalized * minRadius+playerPosition;
            } else
            {
                Vector2 CameraPosition = Vector2.zero;
                MouseScreenPosition = MouseWorldPosition - CameraPosition;
            }
        }
        else
        {
            Vector2 MouseWorldPosition = SharedContent.MousePosition;
            Vector2 CameraPosition = Vector2.zero;
            MouseScreenPosition = MouseWorldPosition - CameraPosition;
        }
        transform.position = MouseScreenPosition;
    }

    protected override void Draw()
    {
        circleRender.startColor = color;
        circleRender.endColor = color;

        circleRender.positionCount = steps;
        Vector2 currentPosition = transform.position;

        float radius = getRadius();
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
        try
        {
            this.radius.SetReferenceValue(radius);
        } catch(System.Exception e)
        {
            //TODO
        }

    }

    public override void setControl(float kp, float ki, float kd)
    {
        KD_CONTROL = kd;
        KI_CONTROL = ki;
        KP_CONTROL = kp;
    }

    public Vector2 GetRandomPositionInTarget()
    {
        float currentRadius = getRadius();
        float randomRadius = Random.Range(0f, currentRadius);
        float angle = Random.Range(0f, 2 * Mathf.PI);

        Vector2 result = transform.position;
        result.x += randomRadius * Mathf.Cos(angle);
        result.y += randomRadius * Mathf.Sin(angle);
        return result;
    }

    protected override void Control(float time)
    {
        radius.ProportionalControl(time, 0.1f);
        radiusVariation.ProportionalControl(time, KP_CONTROL, KI_CONTROL, KD_CONTROL);
    }


    private Vector2 lastPostion;
    [SerializeField] float fatorVariation = 0.5f;
    [SerializeField] float fatorDistance = 10f;
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

        float referenceValue = fatorVariation * (magnitudeDiff);
        if (player != null)
        {
            float dist = (currentPosition - (Vector2) player.transform.position).magnitude;
            referenceValue -= fatorDistance * 1f / (dist*dist);
            MAX_RADIUS = DEFAULT_MAX_RADIUS * ((dist-MIN_RADIUS) * dist) * fatorDistance;
        }

        radiusVariation.SetReferenceValue(referenceValue);
        lastPostion = currentPosition;
    }

    private float MIN_RADIUS = 0.01f;

    private float DEFAULT_MAX_RADIUS = 5f;
    private float MAX_RADIUS;
    public float getRadius()
    {
        float radius = this.radius.GetCurrentValue()+radiusVariation.GetCurrentValue();
        radius = Mathf.Abs(radius);
        if (radius < MIN_RADIUS)
        {
            radius = MIN_RADIUS;
        }
        else if (radius > MAX_RADIUS)
        {
            radius = MAX_RADIUS;
        }
        return radius;
    }

}
