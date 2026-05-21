using UnityEngine;
using System.Collections.Generic;

public class TriggerSpawnMonster : MonoBehaviour
{
    [Header("Monster")]
    public GameObject monsterPrefab;
    public int spawnCount = 3;

    [Header("Spawn Point & Random Radius")]
    public Transform spawnPoint;
    public float spawnRadius = 5f;

    private bool hasSpawned = false;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (hasSpawned) return;

        if (other.CompareTag("Player"))
        {
            SpawnMonsters();
            hasSpawned = true;
        }
    }

    void SpawnMonsters()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPos = new Vector3(
                spawnPoint.position.x + randomPos.x,
                spawnPoint.position.y,
                spawnPoint.position.z + randomPos.y
            );

            GameObject monster = Instantiate(monsterPrefab, spawnPos, spawnPoint.rotation);
            spawnedMonsters.Add(monster);
        }
    }
}
