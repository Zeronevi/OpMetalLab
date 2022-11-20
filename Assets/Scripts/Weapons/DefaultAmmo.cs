using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAmmo : MonoBehaviour
{

    public static int PISTOL_AMMO = 1;
    public static int SUB_AMMO = 2;
    public static int SNIPPER_AMMO = 3;

    public int ammo_amount = 1;
    public int type = 1;
    // Start is called before the first frame update

    [SerializeField] SpriteRenderer render = null;

    private void Start()
    {
        render = transform.GetChild(1).GetComponent<SpriteRenderer>();
        updateColor();
    }
    public void SetType(int type)
    {
        this.type = type;
        updateColor();
    }

    private void updateColor()
    {
        if (type == PISTOL_AMMO) render.color = Color.green;
        else if (type == SUB_AMMO) render.color = Color.red;
        else if (type == SNIPPER_AMMO) render.color = Color.blue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if(collision.gameObject.GetComponent<WeaponInventory>().AddAmmo(ammo_amount, type))
                Destroy(gameObject);
        }
    }

}
