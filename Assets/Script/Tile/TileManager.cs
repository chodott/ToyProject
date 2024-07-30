using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
   private Dictionary<int, NormalTile> tileDictionary = new Dictionary<int, NormalTile>();
    [SerializeField]
    private float coolTime = 5.0f;
    private bool isReady = true;

    
    [SerializeField]
    private GameObject boxPrefab;
    [SerializeField]
    private SpawnWeaponTable spawnWeaponTable;

    private void Start()
    {
        int num = -1;
        foreach(NormalTile tile in FindObjectsOfType<NormalTile>())
        {
            float tilePosZ = tile.transform.position.z;
            if (Mathf.Abs(tilePosZ) <= 0.2f) tileDictionary.Add(++num, tile);
            else tile.GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void Update()
    {
        SpawnItemBox();
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(coolTime);
        isReady = true;
    }

    private void SpawnItemBox()
    {
        if (isReady == false) return;

        int randomIndex = Random.Range(0, tileDictionary.Count);
        Vector3 spawnPos = tileDictionary[randomIndex].transform.position;
        spawnPos.y += 1.0f;

        GameObject SpawnedBox = Instantiate(boxPrefab, spawnPos, Quaternion.identity);
        SpawnedBox.GetComponent<WeaponBox>().Data = spawnWeaponTable.GetRandomWeapon();

        isReady = false;
        StartCoroutine(Respawn());
    }

}
