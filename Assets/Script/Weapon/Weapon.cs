using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private SOWeapon _weaponData;
    private int _bulletCnt = 0;
    public SOWeapon Data
    {
        get { return _weaponData; }
        set { _weaponData = value; }
    }

    public int BulletCnt
    {
        get { return _bulletCnt; }
    }

    private bool _isReady = true;

    public void Equipped(Transform parentTransform)
    {
        transform.SetParent(parentTransform);
        transform.localPosition = _weaponData.EquipPosition;
        transform.localRotation = Quaternion.Euler(_weaponData.EquipRotation);
    }

    public void Shoot()
    { 
        if (_isReady == false) return;
        Vector3 spawnPos = transform.position + transform.forward * 2;
        GameObject bullet = Instantiate(Data.ProjectilePrefab, spawnPos, transform.rotation);
        bullet.GetComponent<Bullet>().Damage = Data.Damage;
        if (++_bulletCnt >= Data.MaxBulletCnt) Destroy(gameObject);
        StartCoroutine(Reload());
        _isReady = false;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(Data.Rate);
        _isReady = true;
    }
}
