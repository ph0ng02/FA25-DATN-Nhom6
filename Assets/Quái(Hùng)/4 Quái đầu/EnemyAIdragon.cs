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

    // THAY ĐỔI: Phân tách 2 Audio Source
    private AudioSource walkAudioSource; // Dùng cho tiếng bước chân (Loop)
    public AudioSource sfxAudioSource;   // Dùng cho Attack/Roar (OneShot)

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

        // THAY ĐỔI: Lấy Audio Source đầu tiên làm tiếng bước chân
        walkAudioSource = GetComponent<AudioSource>();
        // *LƯU Ý: Bạn phải kéo thả Audio Source thứ 2 vào biến sfxAudioSource trong Inspector*

        // Thiết lập AudioSource cho âm thanh bước chân
        walkAudioSource.clip = walkSound;
        walkAudioSource.loop = true;

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
                if (distance <= roarRange && Time.time >= lastRoarTime + roarCooldown)
                {
                    currentState = EnemyState.Roaring;
                    Scream();
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

        ExecuteState();
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
                agent.isStopped = true;
                break;
        }
    }

    // --- HÀM HÀNH ĐỘNG ---

    void UpdateAnimatorAndWalkSound()
    {
        bool isWalking = agent.velocity.magnitude > 0.1f;
        animator.SetBool("IsWalking", isWalking);

        // THAY ĐỔI: Điều khiển âm thanh bước chân bằng walkAudioSource
        if (isWalking && currentState != EnemyState.Roaring)
        {
            if (!walkAudioSource.isPlaying && walkAudioSource.clip == walkSound)
            {
                walkAudioSource.Play();
            }
        }
        else
        {
            if (walkAudioSource.isPlaying && walkAudioSource.clip == walkSound)
            {
                walkAudioSource.Stop();
            }
        }
    }

    void ChasePlayer()
    {
        isChasing = true;
        isWaiting = false;
        agent.isStopped = false;
        agent.SetDestination(player.position);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
        }
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

            switch (enemyType)
            {
                case EnemyType.Golem:
                case EnemyType.Dragon:
                    int attackChoice = Random.Range(0, 2);
                    if (attackChoice == 0) animator.SetTrigger("IsAttacking");
                    else if (attackChoice == 1) animator.SetTrigger("IsAttack2");
                    //else animator.SetTrigger("IsShoot");     
                    break;
                case EnemyType.Hobgoblin:
                case EnemyType.Witch:
                    animator.SetTrigger("IsAttacking");
                    break;
            }

            // THAY ĐỔI: Phát âm thanh Tấn công bằng sfxAudioSource
            if (attackSound != null && sfxAudioSource != null)
            {
                sfxAudioSource.PlayOneShot(attackSound);
            }
        }
    }

    // HÀM SCREAM (ROAR) (Sử dụng Coroutine để tạo độ trễ)
    void Scream()
    {
        lastRoarTime = Time.time;
        agent.isStopped = true;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        animator.SetTrigger("IsScream");

        // THAY ĐỔI: Phát âm thanh Roar bằng sfxAudioSource
        if (roarSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(roarSound);
        }

        StartCoroutine(ScreamDelayAndAttack());
    }

    IEnumerator ScreamDelayAndAttack()
    {
        yield return new WaitForSeconds(roarDelay);
        if (currentState == EnemyState.Roaring)
        {
            currentState = EnemyState.Attacking;
        }
    }

    // ... (Giữ nguyên các hàm Patrol, SetNewPatrolPoint, OnDrawGizmosSelected) ...

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