using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        AMMO_PER_PACKAGE = 30;
        TIME_SHOOTS = 1f;
        TIME_RELOAD = 2f;

        fov_on_target = Cone_vision.NORMAL_FOV-5f;
        viewDistance_on_target = Cone_vision.NORMAL_VIEWDISTANCE + 5f;

        addAmmo(20);
    }

}
