using UnityEngine;
using System.Collections;

public class BoulderSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject boulderPrefab;
    public float rollForce = 3000f;     // 💥 lực lăn mạnh hơn (trước là 400)
    public float extraDownForce = 3000f; // 💥 lực ép xuống để rơi nhanh
    public float destroyAfter = 1000f;     // xóa sau vài giây tránh lag

    [Header("Random Spawn Range (Z Axis)")]
    public float minZ = 0f;
    public float maxZ = 500f;

    [Header("Timing Settings")]
    public float minInterval = 3f;
    public float maxInterval = 4f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnBoulder();
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
    }

    void SpawnBoulder()
    {
        // Random vị trí spawn theo trục Z
        Vector3 spawnPos = transform.position;
        spawnPos.z = Random.Range(minZ, maxZ);

        GameObject boulder = Instantiate(boulderPrefab, spawnPos, transform.rotation);

        Rigidbody rb = boulder.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.mass = 50f; // 💥 nặng hơn
        rb.linearDamping = 0.2f; // ít cản
        rb.angularDamping = 0.05f; // cho lăn mượt

        // Thêm lực mạnh để rơi + lăn nhanh
        rb.AddForce(transform.forward * rollForce, ForceMode.Impulse);
        rb.AddForce(Vector3.down * extraDownForce, ForceMode.Impulse);

        Destroy(boulder, destroyAfter);
    }
}
