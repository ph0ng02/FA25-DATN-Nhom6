using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Knockback")]
    public float knockbackDuration = 0.2f;
    private Rigidbody rb;
    private bool isKnockedback = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, float knockbackForce, Vector3 knockbackDir)
    {
        if (isKnockedback) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} bị đánh {damage} damage! Máu còn lại: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ApplyKnockback(knockbackForce, knockbackDir));
        }
    }

    private IEnumerator ApplyKnockback(float force, Vector3 direction)
    {
        if (rb == null) yield break;

        isKnockedback = true;
        rb.AddForce(direction * force, ForceMode.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector3.zero;
        isKnockedback = false;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} đã chết!");
        // Bạn có thể thêm hiệu ứng, disable input, hoặc respawn tại đây
    }
}
