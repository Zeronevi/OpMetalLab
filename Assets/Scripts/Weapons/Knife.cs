using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        AMMO_PER_PACKAGE = Weapon.IFINITE_AMMO;
        TIME_SHOOTS = 0.5f;
        TIME_RELOAD = 0f;

        fov_on_target = Cone_vision.NORMAL_FOV - 5f;
        viewDistance_on_target = Cone_vision.NORMAL_VIEWDISTANCE + 5f;

        addAmmo(20);
    }

}
