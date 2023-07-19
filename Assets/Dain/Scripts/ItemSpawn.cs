using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject range;
    TerrainCollider rangeCollider;

    // Start is called before the first frame update
    void Awake()
    {
        rangeCollider = range.GetComponent<TerrainCollider>();
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPostion = range.transform.position;
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X/2 ) * -1, (range_X/2 ));
        range_Z = Random.Range((range_Z/2 ) * -1, (range_Z/2 ));
        Vector3 RandomPosition = new Vector3(range_X, 2f, range_Z);

        Vector3 respawnPosition = originPostion + RandomPosition;
        return respawnPosition;
    }

    public GameObject Chest;
    private void Start()
    {
        StartCoroutine(RandomReSpawn_Coroutine());
    }

    IEnumerator RandomReSpawn_Coroutine()
    {
        while (true) 
        { 
        yield return new WaitForSeconds(60f);
            GameObject instantChest = Instantiate(Chest, Return_RandomPosition(), Quaternion.identity);
        }
    }
    
}