using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("References")]
    public Animator animator;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // Animation trúng đòn (nếu có)
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Animation chết
        animator.SetTrigger("Die");

        // Tắt collider sau 1 tí
        GetComponent<Collider>().enabled = false;

        // Destroy enemy sau 3s
        Destroy(gameObject, 3f);
    }
}
