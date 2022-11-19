using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_Target : Target
{
    
    [SerializeField] MainCharacter player;

    private float radius = 3f;

    private int steps = 10;
    private float field_of_attack = 60f;

    private LineRenderer circleRender;


    public override Vector2 GetPositionTarget()
    {
        float dirAngle = transform.rotation.eulerAngles.z;
        Vector2 center = transform.position;
        Vector2 result = center + radius * (new Vector2(Mathf.Cos(dirAngle), Mathf.Sin(dirAngle)));
        return result;

    }

    protected override void UpdatePosition()
    {
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
    }

    public override void setControl(float kp, float ki, float kd)
    {
        //nothing
    }

    public override void setCorrectRadius(float radius)
    {
        //nothing
    }

    protected override void Control(float time)
    {
        //nothing
    }

    protected override void Draw()
    {
        float centerAngle = player.transform.rotation.eulerAngles.z;
        Vector2 currentCenter = player.transform.position;

        circleRender.positionCount = (steps + 1);
        for (int currentStep = 0; currentStep <= steps; currentStep++)
        {
            float circunferenceProgress = (centerAngle + (((float)currentStep) / ((float)steps) - 1f/2f)* field_of_attack) * Mathf.PI/180f;

            Vector2 positionScaled = Vector2.zero;
            positionScaled.x = Mathf.Cos(circunferenceProgress);
            positionScaled.y = Mathf.Sin(circunferenceProgress);

            Vector2 positionStep = currentCenter + radius * positionScaled;

            circleRender.SetPosition(currentStep, positionStep);
        }
    }

    protected override void Start()
    {
        circleRender = GetComponent<LineRenderer>();
    }

    protected override void UpdateOscilation(float time)
    {
        //nothing
    }
}
