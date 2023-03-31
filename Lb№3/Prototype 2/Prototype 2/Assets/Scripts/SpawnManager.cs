using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    private float xSpawnRange = 20;
    private float zSpawnPos = 20;
    private float startDelay = 2;
    private float spawnInterval = 1.5f;
    //public int animalIndex;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomAnimal", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {   
      
    }

    void SpawnRandomAnimal()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnpos = new Vector3(Random.Range(-xSpawnRange, xSpawnRange), 0, zSpawnPos);
        Instantiate(animalPrefabs[animalIndex], spawnpos,animalPrefabs[animalIndex].transform.rotation);
    }
}
