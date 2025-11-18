using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth1 : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public Slider healthSlider; // Gán slider trong Inspector
    public Canvas healthCanvas; // Canvas hướng về Camera
    public GameObject portalPrefab;   // kéo prefab vào đây
    public Transform portalSpawnPoint; // chỗ muốn spawn

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
        // hiệu ứng chết, animation,...
        Instantiate(portalPrefab, portalSpawnPoint.position, portalSpawnPoint.rotation);

        Destroy(gameObject); // xoá boss
    }
}
