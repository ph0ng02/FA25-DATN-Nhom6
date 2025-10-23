using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;
    private NavMeshAgent agent;

    [Header("Stats")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    [Header("Combat")]
    public int damage = 10;
    public float health = 100f;

    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Nếu enemy đang chết hoặc bị đánh thì không di chuyển
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Hit") || state.IsName("Die")) return;

        // Tấn công
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
        // Đuổi theo
        else if (distance <= detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isMoving", true);
        }
        // Đứng yên
        else
        {
            agent.isStopped = true;
            animator.SetBool("isMoving", false);
        }
    }

    // 🩸 Gọi khi enemy bị trúng đòn
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

    // ☠️ Xử lý chết
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
}
