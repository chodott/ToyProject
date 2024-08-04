using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WeaponBox : MonoBehaviour
{
    [SerializeField]
    private SOWeapon _weaponData;
    private GameObject _weapon;

    public SOWeapon Data
    {
        get { return _weaponData; }
        set { _weaponData = value;}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) return;

        _weapon = Instantiate(_weaponData.WeaponPrefab);
        _weapon.GetComponent<Weapon>().Data = _weaponData;
        other.transform.parent.GetComponent<PlayerController>().EquipWeapon(_weapon);
        Destroy(gameObject);
    }
}
