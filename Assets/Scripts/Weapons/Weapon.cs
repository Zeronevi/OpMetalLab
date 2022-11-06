using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private GameObject superior;
    private GameObject lateral;

    public static int IFINITE_AMMO = -1;
    protected int AMMO_PER_PACKAGE = 30;

    private bool isWaiting = false;
    protected float TIME_SHOOTS = 1f;

    private bool isReloading = false;
    protected float TIME_RELOAD = 1.5f;

    private float time = 0f;

    protected int total_ammo = 0;
    protected int ammo_in_weapon = 0;

    protected float bulletSpeed = 25f;
    protected float bulletNoise = 5f;

    protected float speed_on_target = 2f;
    protected float fov_on_target = 20f;
    protected float viewDistance_on_target = 40f;

    protected BoxCollider2D weaponCollider;
    protected virtual void Start()
    {
        this.superior = transform.GetChild(1).gameObject;
        this.lateral = transform.GetChild(0).gameObject;
        this.weaponCollider = GetComponent<BoxCollider2D>();
    }

    public void addAmmo(int ammo)
    {
        this.total_ammo += ammo;
    }

    public bool CanShoot()
    {
        return (time <= 0 && ammo_in_weapon > 0);
    }

    public int GetTotalAmmo()
    {
        return total_ammo + ammo_in_weapon;
    }

    public bool isAvaliable()
    {
        return (!isWaiting);
    }

    public void Reload()
    {
        if (total_ammo > 0 && AMMO_PER_PACKAGE != IFINITE_AMMO)
        {
            isReloading = true;
            time = TIME_RELOAD;
        }
    }

    public void Shoot(GameObject bullet, Vector2 position, Quaternion rotation, Vector2 dir)
    {
        var objBullet = Instantiate(bullet, position, rotation);
        objBullet.GetComponent<Rigidbody2D>().velocity =
            bulletSpeed * dir.normalized;

        NoiseSystem.MakeNoise(transform.position, bulletNoise);

        ammo_in_weapon -= 1;
        time = TIME_SHOOTS;
    }

    public void Equip()
    {
        superior.SetActive(true);
        lateral.SetActive(false);
    }

    public void Unequip()
    {
        superior.SetActive(false);
        lateral.SetActive(false);
    }

    public void Take()
    {
        superior.SetActive(false);
        lateral.SetActive(false);
    }

    public void Drop()
    {
        superior.SetActive(false);
        lateral.SetActive(true);
    }

    public void Hide()
    {
        superior.SetActive(false);
        lateral.SetActive(false);
    }

    public BoxCollider2D GetCollider()
    {
        return weaponCollider;
    }

    protected virtual void Update()
    {

        if(time > 0) time -= Time.deltaTime;

        if(time <= 0 && isReloading)
        {
            total_ammo += ammo_in_weapon;
            ammo_in_weapon = 0;

            if (total_ammo >= AMMO_PER_PACKAGE) ammo_in_weapon = AMMO_PER_PACKAGE;
            else ammo_in_weapon = total_ammo;

            total_ammo -= ammo_in_weapon;

            isReloading = false;
        }

    }

    public void ResetTime()
    {
        isReloading = false;
        isWaiting = false;
        time = 0;
    }
}
