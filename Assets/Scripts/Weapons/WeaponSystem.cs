using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private int MAX_INVENTORY_PLAYER = 5;

    [SerializeField] private WeaponInventory inventoryScene = null;
    [SerializeField] private WeaponInventory playerInventory = null;
    [SerializeField] private Target playerTarget = null;

    private GameObject player;
    private BoxCollider2D playerCollider;

    [SerializeField] public GameObject bullet, gunBarrel;
    [SerializeField] public float bulletSpeed;

    void Start()
    {
        this.player = GameObject.Find("Player");
        this.playerCollider = player.GetComponent<BoxCollider2D>();
        this.playerInventory.HideAll();
        this.inventoryScene.ShowAllDropped();
    }

    private bool shooting = false;
    private bool target = false;
    private bool running = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
        } else if(Input.GetKeyUp(KeyCode.Mouse0))
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            running = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            running = false;
        }

        selectWeapon();
        dropWeapon();
        takeWeapon();
        ReloadWeapon();
        MaybeShoot();
        AdjustParameters();
       
    }

    private void MaybeShoot()
    {
        if (shooting) Shoot();
    }

    private void AdjustParameters()
    {
        if (target && playerInventory.isSelected())
        {
            Weapon weapon = playerInventory.getSelectedWeapon();
            float newFov = weapon.GetFov_on_target();
            float newViewDistance = weapon.GetViewDistance_on_target();
            float newSpeed = weapon.GetSpeed_on_target();

            if (newFov == Weapon.DEFAULT_VALUE) newFov = Cone_vision.NORMAL_FOV;
            if (newViewDistance == Weapon.DEFAULT_VALUE) newViewDistance = Cone_vision.NORMAL_VIEWDISTANCE;
            if (newSpeed == Weapon.DEFAULT_VALUE) newSpeed = MainCharacter.NORMAL_SPEED;

            PlayerStatus.SetVision(newFov, newViewDistance);
            PlayerStatus.SetSpeed(newSpeed);

            playerTarget.setCorrectRadius(weapon.GetRadiusOnTarget());
            playerTarget.setControl(10f, 1f, 10f);
        }
        else
        {
            PlayerStatus.SetVision(Cone_vision.NORMAL_FOV, Cone_vision.NORMAL_VIEWDISTANCE);

            if (running)
            {
                PlayerStatus.SetSpeed(MainCharacter.RUN_SPEED);
            } else
            {
                PlayerStatus.SetSpeed(MainCharacter.NORMAL_SPEED);
            }

            playerTarget.setCorrectRadius(Target.DEFAULT_RADIUS);
            playerTarget.setControl(Target.DEFAULT_KP_CONTROL, Target.DEFAULT_KI_CONTROL, Target.DEFAULT_KD_CONTROL);
        }
    }

    private void selectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            print("Nenhuma arma selecionada!");
            playerInventory.selectThisWeapon(WeaponInventory.NOT_SELECTED);
            return;
        }

        for(int index = 0; index < MAX_INVENTORY_PLAYER; index++)
        {
            if(Input.GetKeyDown(""+(index+1)) && playerInventory.isAvaliable(index))
            {
                playerInventory.selectThisWeapon(index);
                return;
            }
        }
    }

    private void dropWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Backspace) && playerInventory.isSelected())
        {
            Weapon weapon = playerInventory.getSelectedWeapon();
            playerInventory.removeSelectedWeapon();

            inventoryScene.addWeapon(weapon);
        }
    }

    public void takeWeapon()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        if(playerInventory.GetSize() >= MAX_INVENTORY_PLAYER) return;

        for(int index = 0; index < inventoryScene.GetSize(); index++)
        {
            Weapon weapon = inventoryScene.getWeapon(index);
            BoxCollider2D weaponCollider = weapon.GetCollider();
           
            if(weaponCollider.bounds.Intersects(playerCollider.bounds))
            {
                inventoryScene.removeWeapon(index);
                playerInventory.addWeapon(weapon);
                return;
            }
        }

    }

    void Shoot()
    {
        if (!playerInventory.isSelected()) return;

        Weapon weapon = playerInventory.getSelectedWeapon();

        if (!weapon.CanShoot()) return;

        Vector2 positionToFire = playerTarget.GetPositionTarget();  
        weapon.Shoot(this.bullet, gunBarrel.transform.position, bullet.transform.rotation, positionToFire);
    }

    private void ReloadWeapon()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (playerInventory.isSelected()) playerInventory.getSelectedWeapon().Reload();
        }
    }
}
