using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;  // Máu tối đa
    private float currentHealth;     // Máu hiện tại

    [Header("UI Elements")]
    public Slider healthSlider;  // Thanh máu (nếu bạn muốn hiển thị UI)

    [Header("Respawn Settings")]
    public Transform spawnPoint;  // Điểm spawn (kéo Transform vào đây trong Inspector)

    void Start()
    {
        currentHealth = maxHealth;  // Khởi tạo máu ban đầu
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;  // Cập nhật thanh máu UI
            healthSlider.value = currentHealth;  // Cập nhật giá trị thanh máu
        }
    }

    void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;  // Cập nhật lại thanh máu trong mỗi frame
        }

        // Kiểm tra nhấn phím T để thử sát thương
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f);  // Trừ 10 máu mỗi lần nhấn T
        }

        // Kiểm tra nhấn phím H để thử hồi máu
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10f);  // Hồi 10 máu mỗi lần nhấn H
        }

        // Kiểm tra xem nhân vật có chết hay không (máu <= 0)
        if (IsDead())
        {
            Respawn();  // Nếu chết, respawn về điểm spawn
        }
    }

    // Hàm nhận sát thương
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // Bạn có thể thêm hiệu ứng mất máu ở đây như âm thanh, hiệu ứng hình ảnh
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
    }

    // Hàm hồi máu
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        // Thêm hiệu ứng hồi máu nếu cần
        Debug.Log("Player healed by " + amount + ". Current health: " + currentHealth);
    }

    // Kiểm tra xem nhân vật có chết hay không
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // Hàm respawn nhân vật về điểm spawn nhưng không hồi máu
    private void Respawn()
    {
        if (spawnPoint != null)
        {
            // Di chuyển nhân vật về điểm spawn
            transform.position = spawnPoint.position;

            // Không hồi máu, máu vẫn giữ nguyên (vẫn là 0)
            // Không thay đổi currentHealth

            // Cập nhật lại thanh máu (nếu có)
            if (healthSlider != null)
            {
                healthSlider.value = currentHealth;  // Máu vẫn là 0
            }

            Debug.Log("Player respawned at the spawn point. Current health remains at 0.");
        }
        else
        {
            Debug.LogWarning("Spawn point is not assigned!");
        }
    }
}
