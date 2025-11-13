using UnityEngine;
using UnityEngine.UI;

public class HealthTest : MonoBehaviour
{
    [Header("Player Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar; // Gắn thanh máu trong UI

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;

        Debug.Log($"💢 Player bị tấn công! Mất {amount} máu. Còn lại {currentHealth} máu.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("💀 Player đã chết!");
        // Tạm thời tắt player
        gameObject.SetActive(false);
    }
}
