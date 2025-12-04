using UnityEngine;

public class CrystalNovaSkill : MonoBehaviour
{
    public GameObject vfxPrefab; // Prefab của skill
    public Transform spawnPoint; // Vị trí tạo skill (thường là trước mặt player)
    public float cooldown = 5f;

    private float nextTimeCast = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Bấm Q để dùng skill
        {
            CastSkill();
        }
    }

    void CastSkill()
    {
        if (Time.time < nextTimeCast) return;

        // Tạo hiệu ứng tại vị trí spawnPoint
        GameObject vfx = Instantiate(vfxPrefab, spawnPoint.position, spawnPoint.rotation);

        // Tự hủy sau 5 giây (tuỳ theo duration của particle)
        Destroy(vfx, 5f);

        nextTimeCast = Time.time + cooldown;
    }
}
