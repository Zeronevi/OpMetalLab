using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private int MAX_INVENTORY_PLAYER = 5;

    [SerializeField] private WeaponInventory inventoryScene = null;
    [SerializeField] private WeaponInventory playerInventory = null;
    [SerializeField] private Target_System targetSystem = null;
    [SerializeField] private TextSpeakSystem textBoxSystem = null;

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

    public void AdjustParameters(bool target, bool running)
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

            Target playerTarget = targetSystem.GetTarget();
            
            if(playerTarget != null)
            {
                playerTarget.setCorrectRadius(weapon.GetRadiusOnTarget());
                playerTarget.setControl(0.7f, 0.3f, 10f);
            }
            
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

            Target playerTarget = targetSystem.GetTarget();

            if (playerTarget != null)
            {
                playerTarget.setCorrectRadius(Target.DEFAULT_RADIUS);
                playerTarget.setControl(Target.DEFAULT_KP_CONTROL, Target.DEFAULT_KI_CONTROL, Target.DEFAULT_KD_CONTROL);
            }

        }
    }

    public void selectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            print("Nenhuma arma selecionada!");
            playerInventory.selectThisWeapon(WeaponInventory.NOT_SELECTED);
            AdjustTarget();
            return;
        }

        for(int index = 0; index < MAX_INVENTORY_PLAYER; index++)
        {
            if(Input.GetKeyDown(""+(index+1)) && playerInventory.isAvaliable(index))
            {
                playerInventory.selectThisWeapon(index);
                AdjustTarget();
                return;
            }
        }
    }

    public void AdjustTarget()
    {
        Weapon weapon = playerInventory.getSelectedWeapon();
        if(weapon == null) 
        {
            targetSystem.SetCurrentTarget(null);
            return;
        }

        Target referenceTarget = weapon.GetTarget();
        if (referenceTarget == null)
        {
            targetSystem.SetDefaultTarget();
        }
        else
        {
            targetSystem.SetCurrentTarget(referenceTarget);
        }

    }

    public void UpdateTarget()
    {
        Weapon weapon = playerInventory.getSelectedWeapon();
        if (weapon == null) return;
        
        if (!weapon.isAvaliable()) targetSystem.SetColor(targetSystem.DISABLE_COLOR);
        else targetSystem.SetColor(targetSystem.NORMAL_COLOR);
    }

    public void dropWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Backspace) && playerInventory.isSelected())
        {
            Weapon weapon = playerInventory.getSelectedWeapon();
            playerInventory.removeSelectedWeapon();
            inventoryScene.addWeapon(weapon);
            AdjustTarget();
        }
    }

    public void takeWeapon()
    {
        
        if(playerInventory.GetSize() >= MAX_INVENTORY_PLAYER) return;

        int targetWeaponIndex = -1;
        Weapon targetWeapon = null;
        bool found = false;
        for(int index = 0; index < inventoryScene.GetSize() & !found; index++)
        {
            Weapon weapon = inventoryScene.getWeapon(index);
            BoxCollider2D weaponCollider = weapon.GetCollider();
           
            if(weaponCollider.bounds.Intersects(playerCollider.bounds))
            {
                targetWeaponIndex = index;
                targetWeapon = weapon;
                found = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && targetWeapon != null)
        {
            inventoryScene.removeWeapon(targetWeaponIndex);
            playerInventory.addWeapon(targetWeapon);
            AdjustTarget();
            return;
        } else if(targetWeapon != null && textBoxSystem != null)
        {
            textBoxSystem.ShowActionBox();
        } else if(textBoxSystem != null)
        {
            textBoxSystem.HideActionBox();
        }

    }

    public void Shoot()
    {
        Target playerTarget = targetSystem.GetTarget();
        Weapon weapon = playerInventory.getSelectedWeapon();
        if (playerTarget == null || !playerInventory.isSelected()) return;

        if (weapon.CanShoot())
        {
            if(weapon.isWhiteWeapon())
            {
                Vector2 positionToFire = playerTarget.GetPositionTarget();
                weapon.Shoot(null, player.transform.position, player.transform.rotation, positionToFire);
                //AudioSystem.GetInstance().Shoot(false, gunBarrel.transform.position);
            } else
            {
                Vector2 positionToFire = playerTarget.GetPositionTarget();
                weapon.Shoot(this.bullet, gunBarrel.transform.position, bullet.transform.rotation, positionToFire);
                AudioSystem.GetInstance().Shoot(false, gunBarrel.transform.position);
            }
            

        } else if(Input.GetKeyDown(KeyCode.Mouse0) && !weapon.isWaiting())
        {
            if(!weapon.isWhiteWeapon()) AudioSystem.GetInstance().Shoot(true, gunBarrel.transform.position);
        }
    }

    public void ReloadWeapon()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (playerInventory.isSelected()) playerInventory.getSelectedWeapon().Reload();
        }
    }
}
