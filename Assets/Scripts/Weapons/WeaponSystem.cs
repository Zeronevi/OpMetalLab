using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private int MAX_INVENTORY_PLAYER = 5;

    [SerializeField] private WeaponInventory inventoryScene;
    [SerializeField] private WeaponInventory playerInventory;

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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
        } else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
        }

        if(shooting) Shoot();

        selectWeapon();
        dropWeapon();
        takeWeapon();
        ReloadWeapon();
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

        Vector2 direciton = (SharedContent.MousePosition - (Vector2) player.transform.position);
        weapon.Shoot(this.bullet, gunBarrel.transform.position, bullet.transform.rotation, direciton);
    }

    private void ReloadWeapon()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (playerInventory.isSelected()) playerInventory.getSelectedWeapon().Reload();
        }
    }
}
