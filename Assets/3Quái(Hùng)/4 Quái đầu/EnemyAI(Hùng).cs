using UnityEngine;
using UnityEngine.AI;

// Thêm dòng này để đảm bảo đối tượng luôn có AudioSource
[RequireComponent(typeof(AudioSource))] // <-- THÊM DÒNG NÀY
public class EnemyAIH : MonoBehaviour
{
    [Header("References")]
    public Transform player; //
    private NavMeshAgent agent; //
    private Animator animator; //
    public AudioClip attackSound; // <-- Thêm biến cho SoundAttack
    private AudioSource audioSource; // <-- 1. Thêm biến cho AudioSource

    [Header("Ranges")]
    public float detectionRange = 10f; //
    public float attackRange = 2f; //
    public float patrolRadius = 8f; //

    [Header("Timing")]
    public float patrolWaitTime = 3f; //
    public float attackCooldown = 1.5f; //

    private Vector3 patrolTarget; //
    private float lastAttackTime; //
    private bool isChasing; //
    private bool isWaiting; //

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //
        animator = GetComponent<Animator>(); //
        audioSource = GetComponent<AudioSource>(); // <-- 2. Lấy component AudioSource
        SetNewPatrolPoint(); //
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position); //

        if (distance <= attackRange) //
        {
            AttackPlayer(); //
        }
        else if (distance <= detectionRange) //
        {
            ChasePlayer(); //
        }
        else
        {
            Patrol(); //
        }

        // --- BẮT ĐẦU PHẦN CẬP NHẬT ---

        // 3. Kiểm tra xem Golem có đang đi không
        bool isWalking = agent.velocity.magnitude > 0.1f;
        animator.SetBool("IsWalking", isWalking); //

        // 4. Thêm logic điều khiển âm thanh dựa trên 'isWalking'
        if (isWalking)
        {
            // Nếu đang đi VÀ âm thanh chưa phát -> thì bật lên
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Nếu đứng yên VÀ âm thanh đang phát -> thì tắt đi
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }        
        // --- KẾT THÚC PHẦN CẬP NHẬT ---
    }

    void ChasePlayer()
    {
        isChasing = true; //
        isWaiting = false; //
        agent.isStopped = false; //
        agent.SetDestination(player.position); //
        animator.SetBool("IsAttacking", false); //
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        animator.SetBool("IsAttacking", true);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            Debug.Log("Enemy attacks!");

            // --- THÊM VÀO ĐÂY ---
            // Kiểm tra xem có gán clip chưa rồi mới phát
            if (attackSound != null)
            {
                // PlayOneShot là cách hoàn hảo để phát âm thanh 1 lần
                // mà không cần quan tâm đến cài đặt Loop hay clip chính
                audioSource.PlayOneShot(attackSound);
            }
            // --- KẾT THÚC PHẦN THÊM ---

            // TODO: Gây sát thương thật ở đây
        }
    }

    void Patrol()
    {
        animator.SetBool("IsAttacking", false); //

        if (isChasing) //
        {
            isChasing = false; //
            isWaiting = false; //
            SetNewPatrolPoint(); //
        }

        if (!isWaiting && Vector3.Distance(transform.position, patrolTarget) < 1f) //
        {
            isWaiting = true; //
            agent.isStopped = true; //
            Invoke(nameof(SetNewPatrolPoint), patrolWaitTime); //
        }

        if (!isWaiting) //
        {
            agent.isStopped = false; //
            agent.SetDestination(patrolTarget); //
        }
    }

    void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius + transform.position; //
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas)) //
        {
            patrolTarget = hit.position; //
        }

        isWaiting = false; //
        agent.isStopped = false; //
        agent.SetDestination(patrolTarget); //
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; //
        Gizmos.DrawWireSphere(transform.position, detectionRange); //
        Gizmos.color = Color.red; //
        Gizmos.DrawWireSphere(transform.position, attackRange); //
    }
}