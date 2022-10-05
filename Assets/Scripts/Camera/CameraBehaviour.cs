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
    }


    private void Update(){
        if (_canPlayerChangeTarget){
            _target = (Input.GetKey(KeyCode.LeftShift))?(LookAt.ExtendedVision):(LookAt.Player);
        }

        MaintainPosition();//TODO: Rename
    }

    
    public void SetPredefinedPath(List<Vector2> pathPoints){
        //TODO: Implementar
        throw new NotImplementedException("Moh Otario!!");
    }


    void MaintainPosition(){
        switch (_target){
            default: 
            case LookAt.Player:
                center = player.transform.position;
                break;
            case LookAt.Target:
                break;
            case LookAt.ExtendedVision:
                center = (SharedContent.MousePosition + (Vector2)player.transform.position)/2;
                break;
        }
        center.z = CameraHeight;
        transform.position = center;
    }
}
