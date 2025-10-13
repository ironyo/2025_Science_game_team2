using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpawnItem : MonoBehaviour
{
    private float maxTime = 3f;
    private float minTime = 1.5f;
    private float spawnTime = 0f;
    private float currentTime = 0f;
    private ItemPoolManager itemPoolManager;
    

    private void Start()
    {
        itemPoolManager = GetComponent<ItemPoolManager>();
        ReSpawn();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= spawnTime)
        {
            itemPoolManager.SpawnItem(Random.Range(0,2));
            currentTime = 0f;
            ReSpawn();
        }
    }

    private void ReSpawn()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }
}
