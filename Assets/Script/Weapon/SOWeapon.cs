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
    public float Range;

    [Range(0, 100)]
    [SerializeField]
    public float Damage;

    [Range(0, 50)]
    [SerializeField]
    public float Rate;

    [Range(0, 100)]
    [SerializeField]
    public int MaxBulletCnt;

    [Range(0, 10)]
    [SerializeField]
    public int Num;
    public string WeaponName;
    public GameObject WeaponPrefab;
    public GameObject ProjectilePrefab;
}
