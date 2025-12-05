using UnityEngine;

public class EnemyHealths : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Drop Item System")]
    public GameObject[] dropItems;       // danh sách item có thể rớt
    public float dropChance = 50f;       // % tỉ lệ rớt (0 = không rớt, 100 = chắc chắn rớt)
    public float dropForce = 3f;         // lực bắn item ra ngoài

    private Animator anim;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= Mathf.RoundToInt(amount);
        anim.SetTrigger("Hit");

        Debug.Log("Enemy bị đánh! HP còn: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        anim.SetTrigger("Die");

        DropItem();

        Destroy(gameObject, 2f);
    }

    void DropItem()
    {
        // Kiểm tra tỉ lệ rớt
        float randomValue = Random.Range(0f, 100f);
        if (randomValue > dropChance) return;

        if (dropItems.Length == 0) return;

        // Chọn ngẫu nhiên 1 item từ danh sách
        GameObject itemToDrop = dropItems[Random.Range(0, dropItems.Length)];

        // Spawn item tại vị trí enemy
        GameObject item = Instantiate(itemToDrop, transform.position, Quaternion.identity);

        // Nếu item có Rigidbody thì bắn nó văng ra nhẹ
        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(Vector3.up * dropForce, ForceMode.Impulse);
        }
    }
}
