using UnityEngine;
using UnityEngine.UI;

public class HealthManagement : MonoBehaviour, IDamageable
{
    [Header("Player Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("🎯 HealthManagement Start: currentHealth = " + currentHealth);

        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= Mathf.RoundToInt(damage);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    private void Die()
    {
        Debug.Log("Player died.");
        // ❌ Không có canvas chết, không hiệu ứng
        // Nếu bạn muốn respawn hoặc reload scene, báo mình sửa tiếp
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }
}
