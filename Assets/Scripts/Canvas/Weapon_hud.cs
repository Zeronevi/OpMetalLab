using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_hud : MonoBehaviour
{
    [SerializeField] private WeaponInventory playerInventory = null;

    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private TextMeshProUGUI weaponName = null;
    [SerializeField] private Image reference_reload_bar = null;
    [SerializeField] private Image reload_bar = null;
    [SerializeField] private Image weapon_image = null;

    void Start()
    {
        /*reference_reload_bar = transform.GetChild(2).gameObject.GetComponent<Image>();
        reload_bar = transform.GetChild(3).gameObject.GetComponent<Image>();
        text = transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();*/
    }


    void Update()
    {
        UpdateReload();
        UpdateWeapon();
    }

    private void UpdateReload()
    {
        Weapon weapon = playerInventory.getSelectedWeapon();
        Vector3 scale = reload_bar.transform.localScale;
        if (weapon != null && weapon.GetTimeToWait() > 0)
        {
            float MAX_TIME = 0;
            if(weapon.IsReloading())
            {
                MAX_TIME = weapon.GetTimeToReload();
                reload_bar.color = Color.blue;
            } else
            {
                MAX_TIME = weapon.GetTimeToAnotherShoot();
                reload_bar.color = Color.red;
            }
            float porcent = weapon.GetTimeToWait() / MAX_TIME;
            scale.x = porcent * reference_reload_bar.transform.localScale.x;
        } else
        {
            scale.x = 0;
        }
        reload_bar.transform.localScale = scale;
    }

    private void UpdateWeapon()
    {
        Weapon weapon = playerInventory.getSelectedWeapon();
        if(weapon != null)
        {
            weapon_image.sprite = weapon.GetLateralSprite();
            Color color = Color.white;
            color.a = 0.72f;
            weapon_image.color = color;
            weaponName.text = weapon.GetName();
        } else
        {
            weapon_image.sprite = null;
            Color color = Color.white;
            color.a = 0;
            weapon_image.color = color;
            weaponName.text = "No weapon selected!";
        }

        if(weapon == null || weapon.isWhiteWeapon())
        {
            text.SetText("-- // --");
        } else
        {
            if(weapon.GetAmmoInWeapon() <= 0 && weapon.GetPackagesAmount() > 0) text.SetText("R -- "+ weapon.GetTotalAmmoInBag());
            else text.SetText(weapon.GetAmmoInWeapon() + " // " + weapon.GetTotalAmmoInBag());
        }
    }
}
