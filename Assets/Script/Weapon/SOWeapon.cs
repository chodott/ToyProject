using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/ScriptableObject", order = 1)]
public class SOWeapon : ScriptableObject
{
    [SerializeField]
    public Vector3 EquipPosition;
    [SerializeField]
    public Vector3 EquipRotation;

    [Range(0, 100)]
    [SerializeField]
    public float range;

    [Range(0, 100)]
    [SerializeField]
    public float Damage;

    [Range(0, 50)]
    [SerializeField]
    public float rate;

    [Range(0, 10)]
    [SerializeField]
    public int num;
    public string weaponName;
    public GameObject weaponPrefab;

}
