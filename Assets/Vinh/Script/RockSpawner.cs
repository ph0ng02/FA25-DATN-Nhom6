using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnRock), 0f, spawnInterval);
    }

    void SpawnRock()
    {
        Instantiate(rockPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}

