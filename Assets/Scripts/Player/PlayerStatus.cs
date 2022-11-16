using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    public static float MAX_LIFE = 20.0f;
    [SerializeField] private float current_life = MAX_LIFE;

    public static float MAX_ENERGY = 20.0f;
    [SerializeField] private float current_energy = MAX_LIFE;
    [SerializeField] private float speed_energy = 1f;

    public static void SetCurrentLife(float life)
    {
        PlayerStatus playerStatus = GetInstance();
        if (playerStatus != null) playerStatus.current_life = life;
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
        current_life = MAX_LIFE;
        PlayerStatus.playerStatus = this;
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
            print("Não foi definido o cone de visão!");
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
            print("Não foi definido main_character");
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
            print("Não foi definido o inventorio do player!");
        }

        return null;
    }

}
