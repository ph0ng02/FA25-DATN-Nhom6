using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Cần thiết cho Coroutine (Độ trễ)
using Random = UnityEngine.Random;

// 1. Định nghĩa các loại kẻ địch (Enum)
public enum EnemyType { Golem, Hobgoblin, Witch, Dragon }

[RequireComponent(typeof(AudioSource))]
public class EnemyAIdragon : MonoBehaviour
{
    // --- KHAI BÁO BIẾN ---

    [Header("Enemy Configuration")]
    public EnemyType enemyType; // Chọn loại địch trong Inspector

    // Trạng thái AI để kiểm soát hành động (MỚI)
    private enum EnemyState { Patrol, Chase, Roaring, Attacking }
    private EnemyState currentState = EnemyState.Patrol;

    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    // public HealthComponent healthComponent; // Thêm nếu bạn có component HP

    [Header("Audio Clips")]
    public AudioClip attackSound;
    public AudioClip walkSound;
    public AudioClip roarSound;   // Âm thanh Roar/Scream

    [Header("Ranges")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float patrolRadius = 8f;
    public float roarRange = 5f; // Bán kính kích hoạt Roar

    [Header("Timing")]
    public float patrolWaitTime = 3f;
    public float attackCooldown = 1.5f;
    public float roarCooldown = 15f; // Thời gian hồi chiêu Roar
    public float roarDelay = 1.5f; // Độ trễ giữa Scream và Attack (1-2s bạn yêu cầu)

    private Vector3 patrolTarget;
    private float lastAttackTime;
    private float lastRoarTime;
    private bool isChasing;
    private bool isWaiting;

    // --- CÁC HÀM CƠ BẢN ---

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        // healthComponent = GetComponent<HealthComponent>(); // Lấy component HP

        // Thiết lập AudioSource cho âm thanh bước chân
        audioSource.clip = walkSound;
        audioSource.loop = true;

        SetNewPatrolPoint();
        currentState = EnemyState.Patrol;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // LOGIC CHUYỂN TRẠNG THÁI CHÍNH
        if (distance <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
        else if (distance <= detectionRange)
        {
            if (currentState == EnemyState.Patrol || currentState == EnemyState.Chase)
            {
                // Kích hoạt Roar nếu đủ điều kiện
                if (distance <= roarRange && Time.time >= lastRoarTime + roarCooldown)
                {
                    currentState = EnemyState.Roaring;
                    Scream(); // Bắt đầu Scream và Coroutine trễ
                }
                else
                {
                    currentState = EnemyState.Chase;
                }
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
        }

        // THỰC HIỆN HÀNH ĐỘNG DỰA TRÊN TRẠNG THÁI
        ExecuteState();

        // CẬP NHẬT ANIMATOR VÀ ÂM THANH BƯỚC CHÂN
        UpdateAnimatorAndWalkSound();
    }

    // --- HÀM THỰC THI TRẠNG THÁI ---

    void ExecuteState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
            case EnemyState.Roaring:
                // Kẻ địch đứng yên khi Roar
                agent.isStopped = true;
                break;
        }
    }

    // --- HÀM HÀNH ĐỘNG ---

    void UpdateAnimatorAndWalkSound()
    {
        // Điều khiển animation IsWalking/IsRunning
        bool isWalking = agent.velocity.magnitude > 0.1f;
        animator.SetBool("IsWalking", isWalking);
        // animator.SetBool("IsRunning", agent.velocity.magnitude > 4f); // Thêm nếu cần

        // Điều khiển âm thanh bước chân (chỉ khi không Roar)
        if (isWalking && currentState != EnemyState.Roaring)
        {
            if (!audioSource.isPlaying && audioSource.clip == walkSound)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip == walkSound)
            {
                audioSource.Stop();
            }
        }
    }

    void ChasePlayer()
    {
        isChasing = true;
        isWaiting = false;
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    // HÀM TẤN CÔNG (Sử dụng logic Tách Biệt)
    void AttackPlayer()
    {
        agent.isStopped = true;
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            // --- LOGIC TÁCH BIỆT TẤN CÔNG (Sử dụng TRIGGERS) ---
            switch (enemyType)
            {
                case EnemyType.Golem:
                case EnemyType.Dragon: // Giả sử Dragon cũng dùng 3 đòn này
                    int attackChoice = Random.Range(0, 2);

                    if (attackChoice == 0) animator.SetTrigger("IsAttacking"); // Đòn 1
                    else if (attackChoice == 1) animator.SetTrigger("IsAttack2");    // Đòn 2
                    //else animator.SetTrigger("IsShoot");     // Đòn 3
                    break;

                case EnemyType.Hobgoblin:
                case EnemyType.Witch:
                    // Các loại quái vật đơn giản hơn chỉ dùng 1 đòn
                    animator.SetTrigger("IsAttacking");
                    break;
            }
            // --- KẾT THÚC LOGIC TÁCH BIỆT ---

            if (attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }

            // TODO: Gây sát thương
            // if (player.GetComponent<HealthComponent>() != null)
            // {
            //     player.GetComponent<HealthComponent>().TakeDamage(20);
            // }
        }
    }

    // HÀM SCREAM (ROAR) (Sử dụng Coroutine để tạo độ trễ)
    void Scream()
    {
        lastRoarTime = Time.time;
        agent.isStopped = true;

        animator.SetTrigger("IsScream");
        if (roarSound != null)
        {
            audioSource.PlayOneShot(roarSound);
        }

        // Bắt đầu Coroutine để đợi 1.5s rồi chuyển sang tấn công
        StartCoroutine(ScreamDelayAndAttack());
    }

    IEnumerator ScreamDelayAndAttack()
    {
        // Đợi theo biến roarDelay (1.5s)
        yield return new WaitForSeconds(roarDelay);

        // Nếu kẻ địch vẫn trong trạng thái Roaring (chưa bị đánh hay chết)
        if (currentState == EnemyState.Roaring)
        {
            // Chuyển sang tấn công ngay lập tức
            currentState = EnemyState.Attacking;
        }
    }

    // ... (Giữ nguyên Patrol, SetNewPatrolPoint, OnDrawGizmosSelected)
    void Patrol()
    {
        isChasing = false;
        if (!isWaiting && Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            isWaiting = true;
            agent.isStopped = true;
            Invoke(nameof(SetNewPatrolPoint), patrolWaitTime);
        }

        if (!isWaiting)
        {
            agent.isStopped = false;
            agent.SetDestination(patrolTarget);
        }
    }

    void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius + transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
        }
        isWaiting = false;
        agent.isStopped = false;
        agent.SetDestination(patrolTarget);
    }

    // ... (Giữ nguyên OnDrawGizmosSelected)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, roarRange);
    }
}