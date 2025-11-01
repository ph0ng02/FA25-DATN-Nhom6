using UnityEngine;
using UnityEngine.SceneManagement; // Để load lại scene hoặc chuyển scene khác
using UnityEngine.UI;
using System.Collections.Generic;

public class UIheart : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public List<Image> hearts;

    public int currentHealth = 5;
    public int maxHealth = 5;

    public GameObject gameOverUI; // ← Gán UI Game Over trong Inspector

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("vachammatmau");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();
    }

    void GameOver()
    {
        Debug.Log("Game Over");
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // Hiện UI Game Over nếu có
        }
        Time.timeScale = 0f; // Dừng game
        // Hoặc dùng: SceneManager.LoadScene("GameOverScene");
    }
}


