using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Animator animator;

    public bool IsDead { get; private set; } = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        currentHealth -= Mathf.RoundToInt(damage);

        if (animator != null)
            animator.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        IsDead = true;

        if (animator != null)
            animator.SetTrigger("Die");

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 3f);
    }
}
