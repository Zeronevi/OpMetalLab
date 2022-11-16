using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuritySystem : MonoBehaviour
{
    [SerializeField] List<Vision> camerasSeguran�a = new List<Vision>();

    [SerializeField] bool controlled = false;

    private bool showing = false;

    public void Update()
    {
        if (controlled && Input.GetKeyDown(KeyCode.C))
        {
            if(showing)
            {
                HideAll();
                showing = false;
            } else
            {
                ShowAll();
                showing = true;
            }
        }
    }

    public void ShowAll()
    {
        foreach (Vision camera in camerasSeguran�a) camera.Show();
    }

    public void HideAll()
    {
        foreach (Vision camera in camerasSeguran�a) camera.Hide();
    }
}
