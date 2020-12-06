using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform zombiesParent;
    public float spawnRatio = 1f;
    public int zombiesLimit = 10;
    [SerializeField] private List<Transform> zombieSpawns;

    private float spawnTimer = 0f;

    void Start()
    {
        SpawnZombies();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnRatio)
        {
            SpawnZombies();
            spawnTimer = 0f;
        }
    }

    private void SpawnZombies()
    {
        if(zombiesParent.childCount < zombiesLimit)
        {
            foreach (var spawn in zombieSpawns)
            {
                GameObject newZombie = Instantiate(zombiePrefab, spawn.position, spawn.rotation, zombiesParent);
            }
        }        
    }
}
