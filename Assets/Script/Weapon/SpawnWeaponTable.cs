using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/ScriptableObjectTable", order = 1)]
public class SpawnWeaponTable : ScriptableObject
{
    [System.Serializable]
    public class Weapon
    {
        public SOWeapon weapon;
    }

    public List<Weapon> weapons = new List<Weapon>(); 

    public SOWeapon GetRandomWeapon()
    {
        int index = Random.Range(0, weapons.Count);
        return weapons[index].weapon;
    }
}
