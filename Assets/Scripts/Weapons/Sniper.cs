using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        AMMO_PER_PACKAGE = 12;
        TIME_SHOOTS = 5f;
        TIME_RELOAD = 3f;
        bulletSpeed = 50f;

        addAmmo(25);
    }

}
