using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Transform player;

    private EnemyHealth health;

    [Header("Damage Settings")]
    public float baseDamage = 10f;
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

    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<EnemyHealth>();

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void Update()
    {
        if (health.IsDead) return;

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (!visionTrigger.playerInVision)
        {
            Patrol();
            return;
        }

        player = visionTrigger.player;

        agent.updatePosition = true;
        agent.updateRotation = true;

        agent.SetDestination(player.position);
        animator.SetBool("isMoving", true);

        if (attackTrigger.playerInAttackRange && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

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

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (canJumpAttack)
        {
            animator.SetTrigger("jumpAttack");
            JumpAttack();
            canJumpAttack = false;
            yield return new WaitForSeconds(jumpAttackCooldown);
        }

        animator.SetTrigger("attack");
        yield return new WaitForSeconds(3f);

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
            // Player sử dụng HealthManagement, không phải PlayerHealth
            HealthManagement pm = player.GetComponent<HealthManagement>();
            if (pm != null)
            {
                pm.TakeDamage(baseDamage);
            }
        }
    }
}
