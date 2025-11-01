using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Thanh máu dạng thanh trượt (Slider)")]
    public Slider healthSlider;
    public TMP_Text HealthsliderText;

    [Header("Máu & cấu hình")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float damageAmount = 10f;

    [Header("Trái tim UI")]
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public List<Image> hearts; // Kéo trái tim vào đây

    [Header("UI Game Over")]
    public GameObject gameOverUI;



    private void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;

        }
        UpdateHearts();
        UpdateHealthsliderText();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Cập nhật Slider
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        UpdateHearts();
        UpdateHealthsliderText();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        UpdateHearts();
    }

    private void UpdateHearts()
    {
        if (hearts.Count == 0) return;

        int heartHealth = Mathf.CeilToInt(currentHealth / (maxHealth / hearts.Count));

        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < heartHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    void GameOver()
    {
        Debug.Log("Player chết!");
        Time.timeScale = 0f;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Gọi respawn
        PlayerRespawn respawn = GetComponent<PlayerRespawn>();
        if (respawn != null)
        {
            Time.timeScale = 1f; // bật lại thời gian để coroutine hoạt động
            respawn.Die();
        }
    }

    void UpdateHealthsliderText()
    {
        if (HealthsliderText != null)
            HealthsliderText.text = Mathf.RoundToInt(currentHealth) + " / " + Mathf.RoundToInt(maxHealth);
    }


}


