using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class HealthManagement : MonoBehaviour, IDamageable
{
    [Header("Player Health")]
    [SerializeField] private int maxHealth = 200;
    [SerializeField] private int currentHealth;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    [SerializeField] private DieCanvasController dieCanvasController;

    [Header("Damage Effect UI")]
    [SerializeField] private Image damageEffectImage;
    [SerializeField] private float damageEffectTime = 0.5f;

    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("🎯 HealthManagement Start: currentHealth = " + currentHealth);

        UpdateHealthUI();

        if (damageEffectImage != null)
            damageEffectImage.enabled = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= Mathf.RoundToInt(damage);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Hiện hiệu ứng đỏ
        if (damageEffectImage != null)
            StartCoroutine(ShowDamageEffect());

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

        if (dieCanvasController != null)
        {
            dieCanvasController.ShowDieCanvas();
        }
        else
        {
            Debug.LogError("DieCanvasController is not assigned!");
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private System.Collections.IEnumerator ShowDamageEffect()
    {
        damageEffectImage.enabled = true;
        yield return new WaitForSeconds(damageEffectTime);
        damageEffectImage.enabled = false;
    }

    public void SetHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

}
