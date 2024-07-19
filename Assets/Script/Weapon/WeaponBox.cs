using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WeaponBox : MonoBehaviour
{
    [SerializeField]
    private SOWeapon weaponData;
    private GameObject weapon;

    public SOWeapon Data
    {
        get { return weaponData; }
        set 
        { 
            weaponData = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        weapon = Instantiate(weaponData.weaponPrefab);
        weapon.GetComponent<Weapon>().Data = weaponData;
        other.transform.parent.GetComponent<PlayerController>().EquipWeapon(weapon);
        Destroy(gameObject);
    }
}
