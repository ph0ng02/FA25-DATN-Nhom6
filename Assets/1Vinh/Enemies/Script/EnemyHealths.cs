using UnityEngine;
using UnityEngine.UI;

public class EnemyHealths : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Health Bar UI")]
    public Slider healthSlider;      // kéo slider vào đây

    [Header("Drop Item System")]
    public GameObject[] dropItems;   // danh sách item có thể rớt
    public float dropChance = 50f;   // % tỉ lệ rớt
    public float dropForce = 3f;     // lực bắn item ra

    private Animator anim;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        // Setup Slider
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

        Debug.Log("Enemy bị đánh! HP còn: " + currentHealth);

        // Cập nhật thanh máu
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (anim != null)
            anim.SetTrigger("Die");

        DropItem();

        // Xoá thanh máu khi chết
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
