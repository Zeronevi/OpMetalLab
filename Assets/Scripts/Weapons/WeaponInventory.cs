using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public static int NOT_SELECTED = -1;
    public static int NOT_FOUND = -2;

    [SerializeField] private List<Weapon> weapons;
    private int indexSelectedWeapon;

    [SerializeField] bool isPlayer;
    void Start()
    {
        indexSelectedWeapon = NOT_SELECTED;
    }

    public int GetSize()
    {
        return weapons.Count;
    }

    public Weapon getSelectedWeapon()
    {
        if (indexSelectedWeapon == NOT_SELECTED) return null;
        else return weapons[indexSelectedWeapon];
    }

    public Weapon getWeapon(int index)
    {
        return weapons[index];
    }

    public bool isAvaliable(int index)
    {
        return (index >= 0 && index < weapons.Count);
    }

    public bool selectThisWeapon(int index)
    {
        if(indexSelectedWeapon != NOT_SELECTED)
        {
            weapons[indexSelectedWeapon].Unequip();
            indexSelectedWeapon = NOT_SELECTED;
        }


        if (index != NOT_SELECTED)
        {            
            weapons[index].Equip();
            weapons[index].ResetTime();
            indexSelectedWeapon = index;
        }

        return true;
    }

    public void addWeapon(Weapon weapon)
    {
        weapons.Add(weapon);

        if (isPlayer && weapons.Count == 1) selectThisWeapon(0);
        else if (!isPlayer) weapon.Drop();
    }

    public void removeSelectedWeapon()
    {
        weapons[indexSelectedWeapon].Unequip();
        weapons[indexSelectedWeapon].Take();
        weapons.RemoveAt(indexSelectedWeapon);
        indexSelectedWeapon = NOT_SELECTED;
    }

    public void removeWeapon(int index)
    {
        weapons[index].Unequip();
        weapons[index].Take();
        weapons.RemoveAt(index);
        indexSelectedWeapon = NOT_SELECTED;
    }

    public int findeWeapon(Weapon weapon)
    {
        if(weapons.Contains(weapon))
        {
            return weapons.IndexOf(weapon);
        }
        return NOT_FOUND;
    }
    private void Update()
    {
        if (isPlayer && indexSelectedWeapon != NOT_SELECTED)
        {
            weapons[indexSelectedWeapon].transform.position = transform.position;
            weapons[indexSelectedWeapon].transform.rotation = transform.rotation;
        }
    }

    public bool isSelected()
    {
        return (indexSelectedWeapon != NOT_SELECTED);
    }

    public void HideAll()
    {
        if (GetSize() <= 0) return;
        indexSelectedWeapon = NOT_SELECTED;
        foreach (Weapon weapon in weapons)
        {
            weapon.Hide();
        }
    }

    public void ShowAllDropped()
    {
        if (GetSize() <= 0) return;
        foreach (Weapon weapon in weapons)
        {
            weapon.Drop();
        }
    }
}
