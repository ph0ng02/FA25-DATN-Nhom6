using UnityEngine;
using UnityEngine.AI; // Cần thiết để tắt NavMeshAgent khi chết

public class EnemyHealthAI : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Core Components")]
    private Animator animator;
    private NavMeshAgent agent;
    private Collider mainCollider;
    private bool isDead = false;

    // THAM CHIẾU ĐẾN SCRIPT AI ĐỂ TẮT NÓ KHI CHẾT
    // Chúng ta dùng "MonoBehaviour" để nó có thể nhận BẤT KỲ script AI nào
    [Header("AI Script Reference")]
    public MonoBehaviour aiScript; // Kéo script EnemyAIdragon hoặc EnemyAIH vào đây

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        mainCollider = GetComponent<Collider>();
    }

    // HÀM NHẬN SÁT THƯƠNG (CÔNG KHAI)
    public void TakeDamage(int damageAmount)
    {
        // Nếu đã chết, không nhận thêm sát thương
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Bị đánh nhưng chưa chết -> Kích hoạt animation GetHit
            // (Đảm bảo IsGetHit là TRIGGER trong Animator của bạn)
            animator.SetTrigger("IsGetHit");
        }
    }

    // HÀM XỬ LÝ KHI CHẾT
    void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " has died.");

        // 1. Kích hoạt animation chết (Đảm bảo IsDead là TRIGGER)
        animator.SetTrigger("IsDead");

        // 2. Tắt AI
        if (aiScript != null)
        {
            aiScript.enabled = false;
        }

        // 3. Tắt NavMesh Agent (dừng di chuyển)
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // 4. Tắt Collider (cho phép đi xuyên qua)
        if (mainCollider != null)
        {
            mainCollider.enabled = false;
        }

        // 5. Hủy đối tượng sau 5 giây
        Destroy(gameObject, 5f);
    }
}