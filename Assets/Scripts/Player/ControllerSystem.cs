using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSystem : MonoBehaviour
{
    [SerializeField] WeaponSystem weaponSystem;
    [SerializeField] PlayerStatus playerStatus;

    PauseScript pauseSystem = null;
    void Start()
    {
        if (pauseSystem == null) pauseSystem = FindObjectOfType<PauseScript>();
    }

    private bool shooting = false;
    private bool target = false;
    private bool running = false;
    void Update()
    {
        if (pauseSystem != null && pauseSystem.IsPaused()) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            target = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            target = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && target == false)
        {
            running = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            running = false;
        }

        weaponSystem.selectWeapon();
        weaponSystem.dropWeapon();
        weaponSystem.takeWeapon();
        weaponSystem.ReloadWeapon();
        weaponSystem.AdjustParameters(target, running);
        weaponSystem.UpdateTarget();
        if (shooting) weaponSystem.Shoot();
    }

    private void FixedUpdate()
    {
        consumeEnergy(Time.deltaTime);
    }

    private float SPEED_TO_RUN = 4f;
    private float SPEED_TO_TARGET = 2f;
    public void consumeEnergy(float deltaTime)
    {
        if(running)
        {
            float energy = SPEED_TO_RUN * deltaTime;
            if (PlayerStatus.hasEnergy(energy)) PlayerStatus.takeSomeEnergy(SPEED_TO_TARGET * deltaTime);
            else running = false;

        } else if(target)
        {
            float energy = SPEED_TO_TARGET * deltaTime;
            if (PlayerStatus.hasEnergy(energy)) PlayerStatus.takeSomeEnergy(SPEED_TO_TARGET * deltaTime);
            else target = false;
        }
    }

    
}
