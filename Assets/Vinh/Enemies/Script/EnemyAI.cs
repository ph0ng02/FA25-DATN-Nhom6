using UnityEngine;
using UnityEngine.AI;

public class EnemyAI1 : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;
    private NavMeshAgent agent;

    [Header("Stats")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    [Header("Combat")]
    public int damage = 10;
    public float health = 100f;
    private bool isDead = false;

    [Header("Vision")]
    public bool playerInVision = false; // player đang ở trong vùng tầm nhìn

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Tự tìm player nếu chưa gán
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        // Nếu player chưa vào vùng tầm nhìn thì đứng yên
        if (!playerInVision)
        {
            agent.isStopped = true;
            animator.SetBool("isMoving", false);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // Nếu đang bị đánh hoặc đang chết thì không di chuyển
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Hit") || state.IsName("Die")) return;

        // Tấn công nếu trong tầm
        if (distance <= attackRange)
        {
            agent.isStopped = true;
            animator.SetBool("isMoving", false);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                animator.SetTrigger("attack");
                lastAttackTime = Time.time;
            }
        }
        // Nếu player ở trong vùng tầm nhìn (trigger) nhưng chưa đủ gần để đánh
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isMoving", true);
        }
    }

    // 🩸 Khi enemy bị trúng đòn
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger("hit");

        if (health <= 0f)
        {
            Die();
        }
    }

    // ☠️ Khi enemy chết
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("die");
        animator.SetBool("isMoving", false);
        agent.isStopped = true;

        // Xóa enemy sau 3 giây
        Destroy(gameObject, 3f);
    }

    // 🔪 Gọi từ Animation Event trong clip Attack
    public void DealDamage()
    {
        if (isDead) return;

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
    }

    // 👀 Khi Player đi vào vùng tầm nhìn (Sphere Collider Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInVision = true;
        }
    }

    // 👀 Khi Player rời khỏi vùng tầm nhìn
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInVision = false;
        }
    }
}
