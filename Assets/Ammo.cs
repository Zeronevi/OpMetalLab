using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    // Start is called before the first frame update
    public int ammos = 10;
    void Start()
    {
        
    }

    public int get_ammo()
    {
        return ammos;
    }

    public void Destrs()
    {
        Destroy(this.gameObject);
    }
}
