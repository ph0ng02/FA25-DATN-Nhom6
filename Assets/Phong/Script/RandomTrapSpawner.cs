using UnityEngine;
using System.Collections;

public class RandomTrapSpawner : MonoBehaviour
{
    [Header("Cài đặt Bẫy")]
    public GameObject trapPrefab; // Kéo Prefab thùng vào đây
    public float minTime = 1f;    // Thời gian chờ ngắn nhất
    public float maxTime = 3f;    // Thời gian chờ dài nhất
    
    [Header("Lực đẩy")]
    public float rollForce = 500f; // Lực đẩy thùng (chỉnh số này to nhỏ tùy ý)
    
    [Header("Độ lệch vị trí")]
    public float xRandomOffset = 1f; // Lệch trái phải

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);
            SpawnTrap();
        }
    }

    void SpawnTrap()
    {
        // 1. Tính vị trí sinh ra
        float randomX = Random.Range(-xRandomOffset, xRandomOffset);
        Vector3 spawnPos = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);

        // 2. Sinh ra thùng và lưu nó vào biến 'newBarrel' để điều khiển
        GameObject newBarrel = Instantiate(trapPrefab, spawnPos, transform.rotation);
        
        // 3. Lấy Rigidbody của cái thùng vừa sinh ra
        Rigidbody rb = newBarrel.GetComponent<Rigidbody>();

        // 4. Nếu thùng có Rigidbody, đẩy nó về phía trước (hướng mũi tên xanh của Spawner)
        if (rb != null)
        {
            // transform.forward nghĩa là hướng phía trước của vật Spawner này
            rb.AddForce(transform.forward * rollForce);
        }
    }
}