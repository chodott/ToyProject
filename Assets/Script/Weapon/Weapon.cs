using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private SOWeapon weaponData;
    public SOWeapon Data
    {
        get { return weaponData; }
        set { weaponData = value; }
    }

    public void Equipped(Transform parentTransform)
    {
        transform.SetParent(parentTransform);
        transform.localPosition = weaponData.EquipPosition;
        transform.localRotation = Quaternion.Euler(weaponData.EquipRotation);
    }
}
