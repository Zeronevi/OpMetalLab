using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    [SerializeField] GameObject knife_bullet = null; 

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        AMMO_PER_PACKAGE = Weapon.IFINITE_AMMO;
        whiteWeapon = true;
        /*TIME_SHOOTS = 0.5f;
        TIME_RELOAD = 0f;

        fov_on_target = 70f;
        viewDistance_on_target = 5f;*/

    }

    public override void Shoot(GameObject bullet, Vector2 position, Quaternion rotation, Vector2 positionToFire)
    {
        GameObject knife_bullet = Instantiate(this.knife_bullet, position, Quaternion.identity);
        knife_bullet.GetComponent<Knife_bullet>().updateParameters(position, 3f, rotation.eulerAngles.z, 60f);
        time = TIME_SHOOTS;
    }

}
