using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject rabbitPrefab;
    public GameObject deerPrefab;
    public GameObject wolfPrefab;

    public int rabbitCount = 20;
    public int deerCount = 7;
    public int wolfCount = 8;

    public Vector2 xRange = new Vector2(8.3f, -39f);
    public Vector2 zRange = new Vector2(40f, -35f);
    public float yValue = 3f;

    void Start()
    {
        SpawnAnimals(rabbitPrefab, rabbitCount);
        SpawnAnimals(deerPrefab, deerCount);
        SpawnAnimals(wolfPrefab, wolfCount);
    }

    void SpawnAnimals(GameObject animalPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(animalPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(xRange.y, xRange.x);
        float z = Random.Range(zRange.y, zRange.x);
        return new Vector3(x, yValue, z) + transform.position;
    }
}
