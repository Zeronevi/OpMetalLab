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

    
    private void Start(){
        _target = LookAt.Player;
        reference_deltaPosition = DEFAULT_DELTAPOSITION*Vector2.zero;
        current_deltaPosition = DEFAULT_DELTAPOSITION*Vector2.one;

        reference_zoom = DEFAULT_ZOOM;
        current_zoom = DEFAULT_ZOOM;
    }

    private void FixedUpdate()
    {
        ProportionalControlDeltaCameraPosition(Time.deltaTime);
        ProportionalControlZoomCamera(Time.deltaTime);
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
        float newZoom = reference_zoom + deltaZoom;
        if (newZoom <= MAX_ZOOM && newZoom >= MIN_ZOOM) reference_zoom = newZoom;
        Camera.main.orthographicSize = current_zoom;
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
                reference_deltaPosition = DEFAULT_DELTAPOSITION*Vector2.one;
                break;
            case LookAt.Target:
                break;
            case LookAt.ExtendedVision:
                Vector2 playerPosition = Vector2.zero;
                playerPosition.x = center.x;
                playerPosition.y = center.y;
                reference_deltaPosition = (SharedContent.MousePosition-playerPosition) *3f/5f;
                break;
        }
        center.z = CameraHeight;

        Vector3 newPosition = center;
        newPosition.x += current_deltaPosition.x;
        newPosition.y += current_deltaPosition.y;

        transform.position = newPosition;
    }

    private float DEFAULT_DELTAPOSITION = 0f;
    [SerializeField] private float K_CONTROL_DELTACAMERA = 0.1f;

    private Vector2 reference_deltaPosition;
    private Vector2 current_deltaPosition;

    private void ProportionalControlDeltaCameraPosition(float deltaT)
    {
        Vector2 error = reference_deltaPosition - current_deltaPosition;
        Vector2 force = (K_CONTROL_DELTACAMERA * error);
        current_deltaPosition = Utils.euler(deltaT, current_deltaPosition, force, 0.01f, 0.01f);
    }

    private float DEFAULT_ZOOM = 12f;
    private float MIN_ZOOM = 5f;
    private float MAX_ZOOM = 20f;
    private float K_CONTROL_ZOOM = 0.025f;
    private float speedZoom = -1400f;

    private float reference_zoom;
    private float current_zoom;

    private void ProportionalControlZoomCamera(float deltaT)
    {
        float error = reference_zoom - current_zoom;
        float force = (K_CONTROL_ZOOM * error);
        current_zoom = Utils.euler(deltaT, current_zoom, force, 0.01f, 0.01f);
    }
}
