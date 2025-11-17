using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public Slider healthSlider; // Gán slider trong Inspector
    public Canvas healthCanvas; // Canvas hướng về Camera

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    void Update()
    {
        // Cho thanh máu luôn quay về hướng Camera
        healthCanvas.transform.LookAt(Camera.main.transform);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
