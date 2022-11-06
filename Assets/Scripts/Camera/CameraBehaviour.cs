using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour{
    public Vector3 center;
    public GameObject player;
     
    enum LookAt { Player, ExtendedVision, Target };
    private LookAt _target;
    private bool _canPlayerChangeTarget = true;
    
    private const float CameraHeight = -10f;

    private float DEFAULT_DELTAPOSITION = 0f;
    [SerializeField] private float K_CONTROL_DELTACAMERA = 0.1f;

    private float DEFAULT_ZOOM = 12f;
    private float MIN_ZOOM = 5f;
    private float MAX_ZOOM = 20f;
    private float K_CONTROL_ZOOM = 0.025f;
    private float speedZoom = -1400f;
    private Model1 cameraX, cameraY, cameraZOOM;

    private void Start(){
        _target = LookAt.Player;

        cameraX = new Model1(0.01f, 0.01f, DEFAULT_DELTAPOSITION);
        cameraY = new Model1(0.01f, 0.01f, DEFAULT_DELTAPOSITION);
        cameraZOOM = new Model1(0.01f, 0.01f, DEFAULT_ZOOM);

    }

    private void FixedUpdate()
    {
        cameraX.ProportionalControl(Time.deltaTime, K_CONTROL_DELTACAMERA);
        cameraY.ProportionalControl(Time.deltaTime, K_CONTROL_DELTACAMERA);
        cameraZOOM.ProportionalControl(Time.deltaTime, K_CONTROL_ZOOM);
    }

    private void Update(){
        UpdatePosition();
        UpdateZoom(Time.deltaTime);
    }

    public void UpdatePosition()
    {
        if (_canPlayerChangeTarget)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _target = LookAt.ExtendedVision;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                _target = LookAt.Player;
            }
        }

        MaintainPosition();
    }

    private void UpdateZoom(float deltaT)
    {
        float deltaZoom = Input.GetAxis("Mouse ScrollWheel") *speedZoom*deltaT;
        float newZoom = cameraZOOM.GetReferenceValue() + deltaZoom;
        if (newZoom <= MAX_ZOOM && newZoom >= MIN_ZOOM) cameraZOOM.SetReferenceValue(newZoom);

        Camera.main.orthographicSize = cameraZOOM.GetCurrentValue();
    }
    
    public void SetPredefinedPath(List<Vector2> pathPoints){
        //TODO: Implementar
        throw new NotImplementedException("Moh Otario!!");
    }


    void MaintainPosition(){
        center = player.transform.position;
        switch (_target){
            default: 
            case LookAt.Player:
                cameraX.SetReferenceValue(DEFAULT_DELTAPOSITION);
                cameraY.SetReferenceValue(DEFAULT_DELTAPOSITION);
                break;
            case LookAt.Target:
                break;
            case LookAt.ExtendedVision:
                Vector2 playerPosition = Vector2.zero;
                playerPosition.x = center.x;
                playerPosition.y = center.y;
                Vector2 reference_deltaPosition = (SharedContent.MousePosition-playerPosition) *3f/5f;

                cameraX.SetReferenceValue(reference_deltaPosition.x);
                cameraY.SetReferenceValue(reference_deltaPosition.y);

                break;
        }
        center.z = CameraHeight;

        Vector3 newPosition = center;
        newPosition.x += cameraX.GetCurrentValue();
        newPosition.y += cameraY.GetCurrentValue();

        transform.position = newPosition;
    }


}
