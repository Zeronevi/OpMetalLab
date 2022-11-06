using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sub : Weapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        AMMO_PER_PACKAGE = 120;
        TIME_SHOOTS = 0.05f;
        TIME_RELOAD = 2f;
        bulletSpeed = 30f;

        addAmmo(120);
    }

}
