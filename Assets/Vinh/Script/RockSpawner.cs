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
        // random Z +- tùy bạn chỉnh
        float randomZ = Random.Range(750f, 850f);

        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, randomZ);

        Instantiate(rockPrefab, spawnPos, spawnPoint.rotation);
    }
}

