using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedContent : MonoBehaviour
{
    public static Vector2 MousePosition;
    private Camera _mainCam;
    
    private void Start(){ 
        _mainCam = Camera.main;
       MousePosition = new Vector2(); 
    }
    
    
    private void FixedUpdate()
    {
        MousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
}
