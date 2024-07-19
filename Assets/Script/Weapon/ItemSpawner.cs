using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    //일정 시간에 따라 랜덤한 위치에 상자 스폰

    public SpawnWeaponTable spawnWeaponTable;
    public GameObject boxPrefab;

    public void SpawnBox()
    {
        //상자 프리팹 스폰
        Vector3 spawnPos = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(0.0f, 2.0f), 0);
        GameObject SpawnedBox = Instantiate(boxPrefab,spawnPos, Quaternion.identity);
        SpawnedBox.GetComponent<WeaponBox>().Data = spawnWeaponTable.GetRandomWeapon();
    }    
}
