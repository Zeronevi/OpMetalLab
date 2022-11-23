using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Sprite lateralSprite = null;

    private GameObject lateral;

    [SerializeField] protected Target target = null;

    public static int DEFAULT_VALUE = -10;

    public static int IFINITE_AMMO = -1;
    [SerializeField] protected int AMMO_PER_PACKAGE = 30;

    [SerializeField] protected float TIME_SHOOTS = 1f;

    private bool reloading = false;
    [SerializeField] protected float TIME_RELOAD = 1.5f;

    protected float time = 0f;

    [SerializeField] protected int total_ammo = 0;
    [SerializeField] protected int ammo_in_weapon = 0;

    [SerializeField] protected float bulletSpeed = 25f;
    [SerializeField] protected float bulletDamage = 20f;
    [SerializeField] protected float bulletNoise = 5f;

    [SerializeField] protected float speed_on_target = DEFAULT_VALUE;
    [SerializeField] protected float fov_on_target = DEFAULT_VALUE;
    [SerializeField] protected float viewDistance_on_target = DEFAULT_VALUE;

    [SerializeField] protected float radius_on_target = 0.1f;

    protected BoxCollider2D weaponCollider;

    protected AudioSource jukebox = null;
    [SerializeField]protected int type = 0;


    [SerializeField] string nameWeapon = "";

    public Sprite GetLateralSprite()
    {
        return lateralSprite;
    }

    public string GetName()
    {
        
        return nameWeapon;
    }

    public int GetTypeWeapon()
    {
        return type;
    }
    private void Awake()
    {
        this.lateral = transform.GetChild(0).gameObject;
        this.lateral.GetComponent<SpriteRenderer>().sprite = lateralSprite;

        weaponCollider = GetComponent<BoxCollider2D>();
        jukebox = GetComponentInChildren<AudioSource>();
    }
    protected virtual void Start()
    { 
        this.weaponCollider = GetComponent<BoxCollider2D>();
    }

    public void addAmmo(int ammo)
    {
        this.total_ammo += ammo;
    }

    public virtual bool CanShoot()
    {
        if (isWhiteWeapon())
        {
            return time <= 0;
        }
        else
        {
            return (time <= 0 && ammo_in_weapon > 0);
        }
    }

    public int GetTotalAmmo()
    {
        return total_ammo + ammo_in_weapon;
    }

    public int GetTotalAmmoInBag()
    {
        return total_ammo;
    }

    public bool isAvaliable()
    {
        return  !(time > 0 || (ammo_in_weapon == 0 && !isWhiteWeapon()));
    }

    public void Reload()
    {
        if (total_ammo > 0 && AMMO_PER_PACKAGE != IFINITE_AMMO && ammo_in_weapon < AMMO_PER_PACKAGE)
        {
            reloading = true;
            time = TIME_RELOAD;
        }
    }

    public virtual void Shoot(GameObject bullet, Vector2 position, Quaternion rotation, Vector2 positionToFire)
    {
        Vector2 dir = positionToFire - position;
        
        var objBullet = Instantiate(bullet, position, rotation);

        objBullet.GetComponent<Bullet>().SetDamage(bulletDamage);
        objBullet.GetComponent<Rigidbody2D>().velocity =
            bulletSpeed * dir.normalized;

        NoiseSystem.MakeNoise(transform.position, bulletNoise,Noise.SoundType.Gunshot);
        PlayAudio();

        ammo_in_weapon -= 1;
        time = TIME_SHOOTS;
        shot = true;
    }

    protected void PlayAudio()
    {
        if(jukebox != null) jukebox.Play();
    }

    public void Equip()
    {
        lateral.SetActive(false);
    }

    public void Unequip()
    {
        lateral.SetActive(false);
    }

    public void Take()
    {
        lateral.SetActive(false);
    }

    public void Drop()
    {
        lateral.SetActive(true);
    }

    public void Hide()
    {
        lateral.SetActive(false);
    }

    public BoxCollider2D GetCollider()
    {
        return weaponCollider;
    }

    protected virtual void Update()
    {

        if(time > 0) time -= Time.deltaTime;

        if(time <= 0 && reloading)
        {
            total_ammo += ammo_in_weapon;
            ammo_in_weapon = 0;

            if (total_ammo >= AMMO_PER_PACKAGE) ammo_in_weapon = AMMO_PER_PACKAGE;
            else ammo_in_weapon = total_ammo;

            total_ammo -= ammo_in_weapon;
            reloading = false;
        }

    }

    public void ResetTime()
    {
        if(reloading)
        {
            reloading = false;
            time = 0;
        }
    }

    public float GetSpeed_on_target()
    {
        return speed_on_target;
    }

    public float GetFov_on_target()
    {
        return fov_on_target;
    }

    public float GetViewDistance_on_target()
    {
        return viewDistance_on_target;
    }

    public int GetAmmoInWeapon()
    {
        return ammo_in_weapon;
    }

    public int GetPackagesAmount()
    {
        float packages = ((float) total_ammo /(float) AMMO_PER_PACKAGE);
        return (int) Mathf.Ceil(packages);
    }

    public bool IsReloading()
    {
        return reloading;
    }

    public float GetTimeToAnotherShoot()
    {
        return TIME_SHOOTS;
    }

    public float GetTimeToReload()
    {
        return TIME_RELOAD;
    }

    public float GetTimeToWait()
    {
        return time;
    }

    public float GetRadiusOnTarget()
    {
        return radius_on_target;
    }

    public Target GetTarget()
    {
        return target;
    }

    protected bool whiteWeapon = false;
    public bool isWhiteWeapon()
    {
        return whiteWeapon;
    }

    public bool HasAmmoInWeapon()
    {
        return ammo_in_weapon > 0;
    }

    public bool isWaiting()
    {
        return time > 0;
    }

    protected bool shot = false;
    public bool IsShotAndReset()
    {
        bool temp = shot;
        shot = false;
        return temp;
    } 

}
