using UnityEngine;
using UnityEngine.AI; 

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
    private Vector3 horizontalVelocity; 
    
    private float forcedYPosition; // Vị trí Y cố định để BUỘC Enemy đứng thẳng

    [Header("Attack Damage")]
    public float normalDamage = 10f; 
    public float enragedDamage = 25f; 
    
    [Header("6. Teleport Skill")]
    public float teleportRange = 10f;       
    public float teleportCooldown = 5f;     
    private float teleportTimer;            

    // Trạng thái hành vi
    private bool isChasing = false;
    private bool isAttacking = false;
    
    // --- SETUP START & UPDATE ---
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); 

        currentHealth = maxHealth;
        currentSpeed = slowChaseSpeed;
        attackTimer = attackCooldown;
        
        forcedYPosition = transform.position.y;
        
        teleportTimer = teleportCooldown;
    }

    private void Update()
    {
        if (player == null) return; 
        
        // BỎ QUA LOGIC TRỌNG LỰC GÂY LỖI isGrounded CỨNG ĐẦU
        Vector3 verticalVelocity = Vector3.down * 0.5f; 

        if (isChasing)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= attackRange)
            {
                HandleAttack();
                horizontalVelocity = Vector3.zero;
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            horizontalVelocity = Vector3.zero;
            if (animator != null) animator.SetFloat("Speed", 0f);
        }

        // 3. TỔNG HỢP VẬN TỐC
        Vector3 finalVelocity = horizontalVelocity + verticalVelocity;

        // 4. DI CHUYỂN CUỐI CÙNG
        characterController.Move(finalVelocity * Time.deltaTime);

        // LỆNH BẮT BUỘC ĐỨNG THẲNG TRÊN MẶT ĐẤT
        if (transform.position.y != forcedYPosition)
        {
            transform.position = new Vector3(transform.position.x, forcedYPosition, transform.position.z);
        }

        // Đếm ngược Attack Cooldown
        if (attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
        }
        
        // LOGIC DỊCH CHUYỂN NGẪU NHIÊN (Kỹ năng ẩn)
        if (isChasing)
        {
            if (teleportTimer >= teleportCooldown)
            {
                TeleportRandomly();
                teleportTimer = 0f; // Reset thời gian hồi chiêu
            }
            else
            {
                teleportTimer += Time.deltaTime; 
            }
        }
    }

    // --- CÁC HÀM HÀNH VI ---

    void ChasePlayer()
    {
        if (isAttacking) return;

        // 1. Quay đầu về Player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        
        // 2. TÍNH TOÁN VẬN TỐC NGANG (X và Z)
        horizontalVelocity = transform.forward * currentSpeed;

        // 3. Animation
        if (animator != null) 
        {
            animator.SetFloat("Speed", currentSpeed); 
        }
    }

    void HandleAttack()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (attackTimer >= attackCooldown)
        {
            isAttacking = true;
            attackTimer = 0f; 

            // if (animator != null) animator.SetTrigger("Attack");

            Debug.Log($"Enemy tấn công! Sát thương: {GetDamage()}");
            
            isAttacking = false; 
        }
        else
        {
            isAttacking = false;
        }
    }

    void TeleportRandomly()
    {
        Vector3 randomDirection = Random.insideUnitSphere * teleportRange;
        randomDirection += transform.position;

        Vector3 finalPosition = new Vector3(
            randomDirection.x,
            forcedYPosition, 
            randomDirection.z
        );
        
        // if (animator != null) animator.SetTrigger("Teleport"); 
        
        transform.position = finalPosition;
        
        Debug.Log("Enemy đã dịch chuyển ngẫu nhiên!");
    }

    // --- CÁC HÀM TRẠNG THÁI ---
    
    void CheckEnrageState()
    {
        if (!isEnraged && currentHealth <= maxHealth * 0.5f)
        {
            isEnraged = true;
            currentSpeed = fastChaseSpeed;
            Debug.Log("CUỒNG NỘ! Tốc độ dí và sát thương tăng mạnh!");
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("BOSS đã bị tiêu diệt!");
        Destroy(gameObject);
    }

    public float GetDamage()
    {
        return isEnraged ? enragedDamage : normalDamage;
    }

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
            if (animator != null) animator.SetFloat("Speed", 0f);
            Debug.Log("Player ra khỏi vùng, ngừng truy đuổi");
        }
    }
}