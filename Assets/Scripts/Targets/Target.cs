using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour
{

    // Start is called before the first frame update
    protected abstract void Start();

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateOscilation();
        Draw();
    }

    private void UpdatePosition()
    {
        Vector2 MouseWorldPosition = SharedContent.MousePosition;
        Vector2 CameraPosition = Vector2.zero;

        Vector2 MouseScreenPosition = MouseWorldPosition - CameraPosition;
        transform.position = MouseScreenPosition;
    }

    protected abstract void UpdateOscilation();

    protected abstract void Draw();
    
}
