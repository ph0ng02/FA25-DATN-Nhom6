using UnityEngine;

public class EnemyAttack_ : MonoBehaviour
{
    public GameObject fireZonePrefab; // Prefab vùng lửa
    public float fireZoneDuration = 3f; // Thời gian tồn tại

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Hàm này sẽ được gọi từ Animation Event
    public void SpawnFireZone()
    {
        if (player != null && fireZonePrefab != null)
        {
            // Tạo vùng lửa tại vị trí dưới chân player
            Vector3 spawnPos = new Vector3(player.position.x, player.position.y, 0);
            GameObject fire = Instantiate(fireZonePrefab, spawnPos, Quaternion.identity);

            // Tự động xóa sau fireZoneDuration
            Destroy(fire, fireZoneDuration);
        }
    }
}
