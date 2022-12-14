using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_System : MonoBehaviour
{
    [SerializeField] public Target default_target;
    [SerializeField] private Target currentTarget = null;

    [SerializeField] public Color NORMAL_COLOR = Color.red;
    [SerializeField] public Color DISABLE_COLOR = Color.red;

    private void Start()
    {
        if (currentTarget == null) default_target.Disable(); 
    }

    public void SetDefaultTarget()
    {
        SetCurrentTarget(default_target);
    }

    public void SetCurrentTarget(Target target)
    {
        if(currentTarget != null) currentTarget.Disable();
        if(target != null) target.Enable();
        currentTarget = target;
    }

    public Target GetTarget()
    {
        return currentTarget;
    }

    public void SetColor(Color cor)
    {
        currentTarget.SetColor(cor);
    }

}
