using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAmmo : MonoBehaviour
{

    [SerializeField] private Sprite PISTOL_SPRITE = null;
    [SerializeField] private Sprite SUB_SPRITE = null;
    [SerializeField] private Sprite SNIPER_SPRITE = null;

    public static int PISTOL_AMMO = 1;
    public static int SUB_AMMO = 2;
    public static int SNIPPER_AMMO = 3;

    public int ammo_amount = 1;
    public int type = 1;
    // Start is called before the first frame update

    private SpriteRenderer render = null;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        updateColor();
    }
    public void SetType(int type)
    {
        this.type = type;
        updateColor();
    }

    private void updateColor()
    {

        if (type == PISTOL_AMMO) render.sprite = PISTOL_SPRITE;
        else if (type == SUB_AMMO) render.sprite = SUB_SPRITE;
        else if (type == SNIPPER_AMMO) render.sprite = SNIPER_SPRITE;
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
