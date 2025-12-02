using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AdvancedEnemy : MonoBehaviour
{
    [Header("1. References")]
    public Transform player;
    private CharacterController characterController;
    private Animator animator;

    [Header("2. Health & State")]
    public float maxHealth = 150f;
    private float currentHealth;
    private bool isEnraged = false;

    [Header("3. Movement Settings")]
    public float slowChaseSpeed = 1.0f;
    public float fastChaseSpeed = 3.5f;
    public float rotationSpeed = 5f;
    private float currentSpeed;

    [Header("4. Combat Settings")]
    public float attackRange = 2.0f;
    public float attackCooldown = 2.5f;
    private float attackTimer;

    [Header("5. Physics Settings")]
    public float gravity = 20.0f;
    private Vector3 velocity; // tốc độ rơi theo y

    [Header("Attack Damage")]
    public float normalDamage = 10f;
    public float enragedDamage = 25f;

    [Header("6. Teleport Skill")]
    public float teleportRange = 10f;
    public float teleportCooldown = 5f;
    private float teleportTimer;

    // trạng thái
    private bool isChasing = false;
    private bool isAttacking = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        currentSpeed = slowChaseSpeed;
        attackTimer = attackCooldown;
        teleportTimer = teleportCooldown;
    }

    private void Update()
    {
        if (player == null) return;

        // 1️⃣ XỬ LÝ GRAVITY
        if (characterController.isGrounded)
        {
            velocity.y = -0.1f; // giữ enemy đứng vững
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime; // rơi tự nhiên
        }

        // 2️⃣ XỬ LÝ CHASE PLAYER HOẶC IDLE
        if (isChasing)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                HandleAttack();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;

            if (animator != null)
                animator.SetFloat("Speed", 0f);
        }

        // 3️⃣ DI CHUYỂN ENEMY
        Vector3 horizontalVelocity = transform.forward * currentSpeed;
        Vector3 finalVelocity = horizontalVelocity + new Vector3(0, velocity.y, 0);
        characterController.Move(finalVelocity * Time.deltaTime);

        // 4️⃣ UPDATE COOLDOWN
        if (attackTimer < attackCooldown)
            attackTimer += Time.deltaTime;

        if (isChasing)
        {
            if (teleportTimer >= teleportCooldown)
            {
                TeleportRandomly();
                teleportTimer = 0f;
            }
            else
            {
                teleportTimer += Time.deltaTime;
            }
        }
    }

    // -----------------------------
    // CHASE PLAYER
    void ChasePlayer()
    {
        if (isAttacking) return;

        Vector3 dir = (player.position - transform.position).normalized;
        Quaternion look = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * rotationSpeed);

        // Animation walk/run
        if (animator != null)
        {
            float animSpeed = currentSpeed > slowChaseSpeed ? 1f : 0.3f;
            animator.SetFloat("Speed", animSpeed);
        }

        currentSpeed = isEnraged ? fastChaseSpeed : slowChaseSpeed;
    }

    // -----------------------------
    // ATTACK
    void HandleAttack()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (attackTimer >= attackCooldown)
        {
            isAttacking = true;
            attackTimer = 0f;

            if (animator != null)
                animator.SetTrigger("Attack");

            Debug.Log($"Enemy tấn công! Sát thương: {GetDamage()}");

            isAttacking = false;
        }
        else
        {
            isAttacking = false;
        }
    }

    // -----------------------------
    // TELEPORT
    void TeleportRandomly()
    {
        Vector3 randomDir = Random.insideUnitSphere * teleportRange;
        randomDir += transform.position;

        Vector3 finalPos = new Vector3(randomDir.x, transform.position.y, randomDir.z);

        if (animator != null)
            animator.SetTrigger("Teleport");

        transform.position = finalPos;
        Debug.Log("Enemy đã dịch chuyển!");
    }

    // -----------------------------
    // ENRAGE
    void CheckEnrageState()
    {
        if (!isEnraged && currentHealth <= maxHealth * 0.5f)
        {
            isEnraged = true;
            currentSpeed = fastChaseSpeed;

            if (animator != null)
                animator.SetFloat("Speed", 1f);

            Debug.Log("CUỒNG NỘ! Tốc độ và sát thương tăng!");
        }
    }

    // -----------------------------
    // DAMAGE & DIE
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        CheckEnrageState();

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("BOSS bị tiêu diệt!");

        if (animator != null)
            animator.SetTrigger("Die");

        Destroy(gameObject, 2f);
    }

    public float GetDamage()
    {
        return isEnraged ? enragedDamage : normalDamage;
    }

    // -----------------------------
    // TRIGGER PLAYER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            Debug.Log("Player vào vùng, bắt đầu truy đuổi");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;

            if (animator != null)
                animator.SetFloat("Speed", 0f);

            Debug.Log("Player ra khỏi vùng, ngừng truy đuổi");
        }
    }
}
