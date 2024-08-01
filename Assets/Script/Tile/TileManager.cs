using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
   private Dictionary<int, NormalTile> _tileDictionary = new Dictionary<int, NormalTile>();
    [SerializeField]
    private float _coolTime = 5.0f;
    private bool _isReady = true;

    
    [SerializeField]
    private GameObject _boxPrefab;
    [SerializeField]
    private SpawnWeaponTable _spawnWeaponTable;

    private void Start()
    {
        int num = -1;
        foreach(NormalTile tile in FindObjectsOfType<NormalTile>())
        {
            float tilePosZ = tile.transform.position.z;
            if (Mathf.Abs(tilePosZ) <= 0.2f) _tileDictionary.Add(++num, tile);
            else tile.GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void Update()
    {
        SpawnItemBox();
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_coolTime);
        _isReady = true;
    }

    private void SpawnItemBox()
    {
        if (_isReady == false) return;

        int randomIndex = Random.Range(0, _tileDictionary.Count);
        Vector3 spawnPos = _tileDictionary[randomIndex].transform.position;
        spawnPos.y += 1.0f;

        GameObject SpawnedBox = Instantiate(_boxPrefab, spawnPos, Quaternion.identity);
        SpawnedBox.GetComponent<WeaponBox>().Data = _spawnWeaponTable.GetRandomWeapon();

        _isReady = false;
        StartCoroutine(Respawn());
    }

}
