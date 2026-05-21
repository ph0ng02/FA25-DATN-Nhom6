using UnityEngine;
using UnityEngine.AI;

public class AdvancedEnemy : MonoBehaviour
{
    [Header("1. References")]
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("2. Health")]
    public float maxHealth = 150f;
    private float currentHealth;
    private bool isEnraged = false;

    [Header("3. Movement")]
    public float slowChaseSpeed = 1.5f;
    public float fastChaseSpeed = 3.5f;
    public float rotationSpeed = 8f;

    [Header("4. Combat")]
    public float attackRange = 2f;
    public float attackCooldown = 2.5f;
    private float attackTimer;

    [Header("5. Teleport Skill")]
    public float teleportRange = 10f;
    public float teleportCooldown = 5f;
    private float teleportTimer;

    private bool isChasing = false;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        attackTimer = attackCooldown;

        agent.stoppingDistance = attackRange - 0.1f;
        agent.updateRotation = false; // tự xoay mượt hơn
    }

    void Update()
    {
        if (player == null) return;

        // Nếu không đuổi
        if (!isChasing)
        {
            agent.isStopped = true;
            if (animator) animator.SetFloat("Speed", 0f);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // Nếu trong tầm đánh
        if (distance <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            ChasePlayer();
        }

        // Cooldown attack
        if (attackTimer < attackCooldown)
            attackTimer += Time.deltaTime;

        // Cooldown teleport
        teleportTimer += Time.deltaTime;

        if (teleportTimer >= teleportCooldown)
        {
            TeleportRandomly();
            teleportTimer = 0f;
        }
    }

    // -----------------------------
    // MOVEMENT
    // -----------------------------

    void ChasePlayer()
    {
        if (isAttacking) return;

        agent.isStopped = false;
        agent.speed = isEnraged ? fastChaseSpeed : slowChaseSpeed;
        agent.SetDestination(player.position);

        // Tự xoay mượt
        Vector3 dir = agent.velocity.normalized;
        if (dir.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
        }

        if (animator) animator.SetFloat("Speed", agent.speed);
    }

    // -----------------------------
    // ATTACK
    // -----------------------------

    void AttackPlayer()
    {
        agent.isStopped = true;

        // Nhìn vào player
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPos);

        if (animator) animator.SetFloat("Speed", 0f);

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;

            // if (animator) animator.SetTrigger("Attack");

            Debug.Log("Enemy Attack! Damage = " + GetDamage());
        }
    }

    // -----------------------------
    // TELEPORT
    // -----------------------------

    void TeleportRandomly()
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * teleportRange;
        randomPos.y = transform.position.y;

        agent.Warp(randomPos); // Teleport CHUẨN CHO NAVMESH

        Debug.Log("Enemy Teleported!");
    }

    // -----------------------------
    // HEALTH
    // -----------------------------

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (!isEnraged && currentHealth <= maxHealth * 0.5f)
        {
            isEnraged = true;
            Debug.Log("ENRAGED MODE!");
        }

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Enemy Dead");
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return isEnraged ? 25f : 10f;
    }

    // -----------------------------
    // TRIGGER
    // -----------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            Debug.Log("Start Chasing!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            Debug.Log("Stop Chasing");
        }
    }
}
