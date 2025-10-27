using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;

    [Header("Hit feedback")]
    [SerializeField] ParticleSystem hitVFX;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float deathDelay = 0.5f;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        // VFX/SFX
        if (hitVFX != null)
        {
            var v = Instantiate(hitVFX, hitPoint, Quaternion.LookRotation(hitDirection));
            Destroy(v.gameObject, 2f);
        }
        if (audioSource != null && hitSfx != null) audioSource.PlayOneShot(hitSfx);

        // TODO: trigger hurt animation here if you have animator
        var anim = GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // play death animation if any
        var anim = GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Die");

        // simple destroy after delay
        Destroy(gameObject, deathDelay);
    }

    // For debug: draw health in inspector
    void OnGUI()
    {
        // optional: not recommended for production
    }

    // Optional: expose health
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
}
