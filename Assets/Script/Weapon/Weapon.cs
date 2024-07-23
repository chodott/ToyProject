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

    private bool bReady = true;
    public int bulletCnt = 0;

    public void Equipped(Transform parentTransform)
    {
        transform.SetParent(parentTransform);
        transform.localPosition = weaponData.EquipPosition;
        transform.localRotation = Quaternion.Euler(weaponData.EquipRotation);
    }

    public void Shoot()
    { 
        if (bReady == false) return;
        Vector3 spawnPos = transform.position + transform.forward * 2;
        GameObject bullet = Instantiate(Data.projectilePrefab, spawnPos, transform.rotation);
        bullet.GetComponent<Bullet>().damage = Data.Damage;
        if (++bulletCnt >= Data.MaxBulletCnt) Destroy(gameObject);
        StartCoroutine(Reload());
        bReady = false;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(Data.rate);
        bReady = true;
    }
}
