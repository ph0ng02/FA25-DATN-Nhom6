using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthss : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 300;
    private int currentHealth;

    [Header("Health Bar UI")]
    public Slider healthSlider;

    [Header("Teleport Settings")]
    public Transform teleportPoint;      // điểm dịch chuyển
    public bool teleportAtHalfHP = true; // bật tắt teleport
    private bool hasTeleported = false;  // để tránh teleport nhiều lần

    [Header("Drop Item System")]
    public GameObject[] dropItems;
    public float dropChance = 50f;
    public float dropForce = 3f;

    private Animator anim;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= Mathf.RoundToInt(amount);

        if (anim != null)
            anim.SetTrigger("Hit");

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        Debug.Log("Enemy bị đánh! HP còn: " + currentHealth);

        // 🔥 Teleport nếu HP <= 50%
        if (teleportAtHalfHP && !hasTeleported && currentHealth <= maxHealth * 0.5f)
        {
            Teleport();
        }

        if (currentHealth <= 0)
            Die();
    }

    void Teleport()
    {
        if (teleportPoint == null)
        {
            Debug.LogWarning("⚠ Bạn chưa gán TeleportPoint!");
            return;
        }

        hasTeleported = true;

        // Dịch chuyển enemy
        transform.position = teleportPoint.position;

        // Dịch chuyển luôn thanh máu theo
        if (healthSlider != null)
        {
            healthSlider.transform.position = transform.position + Vector3.up * 2f;
        }

        Debug.Log("🔵 Enemy đã dịch chuyển vì còn <= 50% máu!");
    }

    void Die()
    {
        if (anim != null)
            anim.SetTrigger("Die");

        DropItem();

        if (healthSlider != null)
            Destroy(healthSlider.gameObject);

        Destroy(gameObject, 1f);
    }

    void DropItem()
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue > dropChance) return;

        if (dropItems.Length == 0) return;

        GameObject itemToDrop = dropItems[Random.Range(0, dropItems.Length)];

        GameObject item = Instantiate(itemToDrop, transform.position, Quaternion.identity);

        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        }
    }
}
