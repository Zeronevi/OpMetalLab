using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour
{
    public static float DEFAULT_RADIUS = 0.75f;
    public static float DEFAULT_KP_CONTROL = 0.3f;
    public static float DEFAULT_KI_CONTROL = 0.3f;
    public static float DEFAULT_KD_CONTROL = 1f;

    // Start is called before the first frame update
    protected abstract void Start();

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        //UpdateOscilation(Time.deltaTime);
        Draw();
    }

    private void FixedUpdate()
    {
        UpdateOscilation(Time.deltaTime);
        Control(Time.deltaTime);
    }

    protected abstract void UpdatePosition();

    public abstract Vector2 GetPositionTarget();

    public abstract void setCorrectRadius(float radius);

    public abstract void setControl(float kp, float ki, float kd);

    protected abstract void Control(float time);

    protected abstract void UpdateOscilation(float time);

    protected abstract void Draw();
    
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
