using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Transform player;

    [Header("Stats")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;
    private bool damageBoosted = false;

    [Header("Damage Settings")]
    public float baseDamage = 10f;
    private float currentDamage;
    public float jumpAttackForce = 7f;
    public float jumpAttackCooldown = 3f;
    private bool canJumpAttack = true;

    [Header("AI Settings")]
    public float attackRange = 2f;
    public float patrolWaitTime = 2f;

    [Header("Colliders")]
    public VisionTrigger visionTrigger;
    public AttackTrigger attackTrigger;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int patrolIndex = 0;

    [Header("Health UI")]
    public GameObject healthBarPrefab;
    private EnemyHealthBar healthBar;

    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
        currentDamage = baseDamage;

        // Tạo health bar
        if (healthBarPrefab != null)
        {
            GameObject hb = Instantiate(healthBarPrefab);
            healthBar = hb.GetComponent<EnemyHealthBar>();
            if (healthBar != null)
                healthBar.SetTarget(transform);
        }

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void Update()
    {
        if (isDead) return;

        // --- Cập nhật tốc độ cho Animator (Cách 2) ---
        animator.SetFloat("Speed", agent.velocity.magnitude);

        // Boost damage 50%
        if (!damageBoosted && currentHealth <= maxHealth * 0.5f)
        {
            currentDamage *= 1.5f;
            damageBoosted = true;
        }

        // Chưa thấy player → đi tuần
        if (!visionTrigger.playerInVision)
        {
            Patrol();
            return;
        }

        // Đã thấy player → dí theo
        player = visionTrigger.player;

        // KHÔNG STOP AGENT
        agent.updatePosition = true;
        agent.updateRotation = true;

        agent.SetDestination(player.position);
        animator.SetBool("isMoving", true);

        // Nếu Player trong vùng tấn công
        if (attackTrigger.playerInAttackRange && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    // ─────────────────────────────────────────────── PATROL ───────────────────────────────────────────────
    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        animator.SetBool("isMoving", true);

        if (Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) < 1f)
        {
            StartCoroutine(PatrolWait());
        }
    }

    IEnumerator PatrolWait()
    {
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(patrolWaitTime);

        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[patrolIndex].position);
    }

    // ─────────────────────────────────────────────── ATTACK ───────────────────────────────────────────────
    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // Nhảy vồ
        if (canJumpAttack)
        {
            animator.SetTrigger("jumpAttack");
            JumpAttack();
            canJumpAttack = false;
            yield return new WaitForSeconds(jumpAttackCooldown);
        }

        // Đánh thường
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(3.0f); // tuỳ animation

        isAttacking = false;
    }

    void JumpAttack()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;

        rb.isKinematic = false;
        rb.AddForce(dir * jumpAttackForce, ForceMode.Impulse);

        StartCoroutine(StopJumpMovement());
    }

    IEnumerator StopJumpMovement()
    {
        yield return new WaitForSeconds(0.6f);
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    // Animation Event
    public void DealDamage()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage((int)currentDamage, 0, Vector3.zero);
        }
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        animator.SetTrigger("hit");

        // Cập nhật health bar
        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("die");
        agent.isStopped = true;

        if (healthBar != null)
            Destroy(healthBar.gameObject);

        Destroy(gameObject, 3f);
    }
}
