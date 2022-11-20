using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_hud : MonoBehaviour
{
    [SerializeField] private WeaponInventory playerInventory = null;

    private TextMeshProUGUI text = null;
    private Image reference_reload_bar = null;
    private Image reload_bar = null;

    void Start()
    {
        reference_reload_bar = transform.GetChild(2).gameObject.GetComponent<Image>();
        reload_bar = transform.GetChild(3).gameObject.GetComponent<Image>();
        text = transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
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
        if(weapon == null || weapon.isWhiteWeapon())
        {
            text.SetText("-- // --");
        } else
        {
            if(weapon.GetAmmoInWeapon() <= 0 && weapon.GetPackagesAmount() > 0) text.SetText("R to reload");
            else text.SetText("Ammo: " + weapon.GetAmmoInWeapon() + " // " + weapon.GetPackagesAmount());
        }
    }
}
