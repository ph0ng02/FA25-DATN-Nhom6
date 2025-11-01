using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Thanh máu")]
    public Slider healthSlider;

    [Header("Cài đặt máu")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float damageAmount = 10f; // Sát thương mỗi lần chạm Enemy

    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player chết!");
            // Thêm xử lý Game Over ở đây nếu cần
        }
    }

    // Nếu Enemy dùng collider thường
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damageAmount);
        }
    }

    // Nếu Enemy dùng collider là Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(damageAmount);
        }
    }
}


