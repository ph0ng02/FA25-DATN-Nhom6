using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;

    [Header("Stats")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    [Header("Combat")]
    public int damage = 10;
    public float health = 100f;
    private bool isDead = false;

    [Header("Knockback")]
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedback = false;

    [Header("Vision")]
    public bool playerInVision = false;
    private Transform currentTarget;

    [Header("Optional Return")]
    public bool returnToStart = true;
    private Vector3 startPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (isDead || isKnockedback) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Hit") || state.IsName("Die")) return;

        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);

            if (!playerInVision)
            {
                currentTarget = null;
                StopMoving();
                return;
            }

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
            else
            {
                agent.isStopped = false;
                agent.SetDestination(currentTarget.position);
                animator.SetBool("isMoving", true);
            }
        }
        else
        {
            if (returnToStart)
            {
                float distToStart = Vector3.Distance(transform.position, startPosition);
                if (distToStart > 0.5f)
                {
                    agent.isStopped = false;
                    agent.SetDestination(startPosition);
                    animator.SetBool("isMoving", true);
                }
                else
                {
                    StopMoving();
                }
            }
            else
            {
                StopMoving();
            }
        }
    }

    private void StopMoving()
    {
        agent.isStopped = true;
        animator.SetBool("isMoving", false);
    }

    // 🩸 Khi enemy bị trúng đòn
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger("hit");

        if (currentTarget != null)
        {
            Vector3 knockDir = (transform.position - currentTarget.position).normalized;
            StartCoroutine(ApplyKnockback(knockDir));
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator ApplyKnockback(Vector3 direction)
    {
        if (rb == null) yield break;

        isKnockedback = true;
        agent.isStopped = true;

        rb.isKinematic = false;
        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        isKnockedback = false;
        agent.isStopped = false;
    }

    // ☠️ Khi enemy chết
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("die");
        animator.SetBool("isMoving", false);
        agent.isStopped = true;

        Destroy(gameObject, 3f);
    }

    // 🔪 Gọi từ Animation Event trong clip Attack
    public void DealDamage()
    {
        if (isDead || currentTarget == null) return;

        if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            PlayerHealth ph = currentTarget.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
    }

    // 👀 Khi Player đi vào vùng tầm nhìn
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInVision = true;
            currentTarget = other.transform;
        }
    }

    // 👀 Khi Player rời khỏi vùng tầm nhìn
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            if (currentTarget == other.transform)
            {
                playerInVision = false;
                currentTarget = null;
                StopMoving();
            }
        }
    }
}
