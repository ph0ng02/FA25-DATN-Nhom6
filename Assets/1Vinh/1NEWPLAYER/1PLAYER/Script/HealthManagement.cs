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
        PlayerStats ps = GetComponent<PlayerStats>();

        currentHealth = (int)ps.currentHP;
        maxHealth = (int)ps.maxHP;

        UpdateHealthUI();

        if (damageEffectImage != null)
            damageEffectImage.enabled = false;

        Debug.Log("Health loaded: " + currentHealth);

        // 🔥 KIỂM TRA NGAY KHI LOAD SCENE
        if (currentHealth <= 0)
        {
            Debug.Log("HP = 0 on scene load → calling Die()");
            Die();
        }

        // Nếu có dữ liệu trước → LOAD lại
        if (PlayerDataManager.Instance.data.hp > 0)
            currentHealth = PlayerDataManager.Instance.data.hp;
        else
            currentHealth = maxHealth;

        Debug.Log("🎯 Loaded HP = " + currentHealth);

        UpdateHealthUI();

        if (damageEffectImage != null)
            damageEffectImage.enabled = false;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= Mathf.RoundToInt(damage);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        // 👉 SAVE
        PlayerStats ps = GetComponent<PlayerStats>();
        ps.currentHP = currentHealth;
        ps.SaveStats();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Lưu HP
        PlayerDataManager.Instance.data.hp = currentHealth;

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

        // Lưu HP
        PlayerDataManager.Instance.data.hp = currentHealth;

        UpdateHealthUI();
    }
}
