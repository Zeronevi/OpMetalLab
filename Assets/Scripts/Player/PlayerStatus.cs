using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private int keys = 0;

    public void addKey()
    {
        keys++;
    } 

    public int getKeys()
    {
        return keys;
    }

    public static float MAX_LIFE = 200f;

    private float current_life = MAX_LIFE;

    public static float MAX_ENERGY = 20.0f;
    [SerializeField] private float current_energy = MAX_LIFE;
    [SerializeField] private float speed_energy = 1f;
    [SerializeField] private GameFinishControl gameFinish = null;
    public static void SetCurrentLife(float life)
    {
        PlayerStatus playerStatus = GetInstance();
        if (playerStatus != null) playerStatus.current_life = life;
    }

    [SerializeField] private GameObject bloodEffect = null;
    public static void TakeDamage(float damage)
    {
        PlayerStatus playerStatus = GetInstance();
        if (playerStatus != null)
        {
            playerStatus.current_life -= damage;
            if (playerStatus.bloodEffect != null) Destroy(Instantiate(playerStatus.bloodEffect, playerStatus.main_character.transform.position, Quaternion.identity), 1f);
            if (playerStatus.current_life <= 0 && playerStatus.gameFinish != null)
            {
                
                Instantiate(playerStatus.gameFinish, Vector2.zero, Quaternion.identity).Loser();
            }
        }
    }

    public static float getCurrentLife()
    {
        PlayerStatus playerStatus = GetInstance();
        if (playerStatus != null) return playerStatus.current_life;
        else return 0;
    }

    public static float getCurrentEnergy()
    {
        PlayerStatus playerStatus = GetInstance();
        if (playerStatus != null) return playerStatus.current_energy;
        else return 0;
    }

    public static bool hasEnergy(float energy)
    {
        return (energy <= playerStatus.current_energy);
    }

    public static void takeSomeEnergy(float energy)
    {
        playerStatus.current_energy -= energy;
        if (playerStatus.current_energy < 0) playerStatus.current_energy = 0;
    }

    private static PlayerStatus playerStatus = null;
    
    public static PlayerStatus GetInstance()
    {
        return playerStatus;
    }
    void Start()
    {
        PlayerStatus.playerStatus = this;
        ResetStatus();
    }

    private void FixedUpdate()
    {
        current_energy += speed_energy * Time.deltaTime;
        if (current_energy > MAX_ENERGY) current_energy = MAX_ENERGY;
    }

    

    [SerializeField] Cone_vision vision = null; 
    [SerializeField] WeaponInventory player_inventory = null;
    [SerializeField] MainCharacter main_character = null;

    public static void SetVision(float fov, float viewDistance)
    {

        Cone_vision vision = (GetInstance() != null) ? GetInstance().vision : null;
        if(vision != null)
        {
            vision.SetReferenceFov(fov);
            vision.SetReferenceViewDistance(viewDistance);
        } else
        {
            print("N�o foi definido o cone de vis�o!");
        }
    }

    public static void SetSpeed(float speed)
    {
        MainCharacter main_character = (GetInstance() != null) ? GetInstance().main_character : null;
        if (main_character != null)
        {
            main_character.SetSpeed(speed);
        }
        else
        {
            print("N�o foi definido main_character");
        }
    }

    public static Weapon getSelectedWeapon()
    {
        WeaponInventory player_inventory = (GetInstance() != null) ? GetInstance().player_inventory : null;
        if (player_inventory != null)
        {
            return player_inventory.getSelectedWeapon();
        } else
        {
            print("N�o foi definido o inventorio do player!");
        }

        return null;
    }

    public void ResetStatus()
    {
        current_life = MAX_LIFE;
        current_energy = MAX_ENERGY;
        keys = 0;
    }

    

    //animator
    [SerializeField] private Animator animatorBody = null;
    [SerializeField] private Animator animatorFeet = null;

    private bool walking = false;
    private bool running = false;
    private void upadateAnimator()
    {
        if (Time.timeScale == 0) return;

        isWalking();

        if(walking && Input.GetKeyDown(KeyCode.LeftShift) && current_energy > 0)
        {
            running = true;
        } else if (Input.GetKeyUp(KeyCode.LeftShift) || current_energy <= 0)
        {
            running = false;
        }

        bool reloading = false;
        bool shot = false;
        int state;

        Weapon weapon = player_inventory.getSelectedWeapon();
        
        if(weapon == null)
        {
            state = 0;
        } else
        {
            state = weapon.GetTypeWeapon();
            reloading = weapon.IsReloading();
            shot = weapon.IsShotAndReset();
        }
        
       
        animatorBody.SetInteger("State", state);
        animatorBody.SetBool("Walk", walking);
        animatorBody.SetBool("Reload", reloading);
        if (shot) animatorBody.SetTrigger("Shoot");

        animatorFeet.SetBool("Walk", walking);
        animatorFeet.SetBool("Run", running);
    }

    public void isWalking()
    {
        Vector2 velocity = Vector2.zero;
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");
        if(velocity.magnitude <= 0.00001)
        {
            walking = false;
        } else
        {
            walking = true;
        }
    }

    private void Update()
    {
        upadateAnimator();
    }

}
