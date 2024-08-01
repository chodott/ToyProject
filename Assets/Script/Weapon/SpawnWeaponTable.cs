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

    [SerializeField]
    private List<Weapon> Weapons = new(); 

    public SOWeapon GetRandomWeapon()
    {
        int index = Random.Range(0, Weapons.Count);
        return Weapons[index].weapon;
    }
}
