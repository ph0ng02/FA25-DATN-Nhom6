using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyC_Dang : MonoBehaviour
{
    private NavMeshAgent agent; 
    private Animator animator;
    private int isWalkingHash; 
    
    // Khai báo Trạng thái AI MỚI
    public enum AIState { Patrol, Chase, Attack } 
    public AIState currentState = AIState.Patrol; 

    [Header("Target Settings")]
    public Transform playerTarget; 

    [Header("Patrol Settings")]
    public Transform patrolRouteParent; 
    public float waitTime = 3f; 

    [Header("Attack Settings")] // CÀI ĐẶT TẤN CÔNG
    [Tooltip("Khoảng cách dừng lại để bắt đầu tấn công")]
    public float attackRange = 1.5f; 
    [Tooltip("Thời gian chờ giữa các đòn đánh (giây)")]
    public float attackCooldown = 2f; 
    public float damageAmount = 10f; 
    
    // Collider tấn công sẽ được bật/tắt bằng Timer trong HandleAttack()
    public Collider attackCollider; 
    
    private bool isAttacking = false; 
    private int isAttackingHash; 
    
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; 
        }

        if (animator != null)
        {
            isWalkingHash = Animator.StringToHash("IsWalking");
            isAttackingHash = Animator.StringToHash("IsAttacking"); 
        }

        if (patrolRouteParent != null)
        {
            waypoints = new Transform[patrolRouteParent.childCount];
            for (int i = 0; i < patrolRouteParent.childCount; i++)
            {
                waypoints[i] = patrolRouteParent.GetChild(i);
            }
        }
        
        // Đảm bảo Attack Collider bị tắt khi bắt đầu game
        if (attackCollider != null) attackCollider.enabled = false;

        if (agent != null)
        {
             StartCoroutine(FSM()); 
        }
    }

    void Update()
    {
        // Tắt Agent khi đang chờ hoặc đang tấn công
        if (agent != null) agent.isStopped = (isWaiting && currentState == AIState.Patrol) || currentState == AIState.Attack;

        // Điều khiển Animation
        if (animator != null && agent != null)
        {
            // Animation chạy khi đang di chuyển (Patrol hoặc Chase)
            bool isMoving = agent.velocity.magnitude > 0.1f && !isWaiting && currentState != AIState.Attack;
            
            try 
            { 
                animator.SetBool(isWalkingHash, isMoving); 
            }
            catch { }
        }
    }

    IEnumerator FSM()
    {
        while (true)
        {
            switch (currentState)
            {
                case AIState.Patrol:
                    yield return StartCoroutine(PatrolCycle());
                    break;
                case AIState.Chase:
                    yield return StartCoroutine(ChasePlayer());
                    break;
                case AIState.Attack: 
                    yield return StartCoroutine(HandleAttack());
                    break;
            }
            yield return null; 
        }
    }

    IEnumerator PatrolCycle()
    {
        // ... (Logic Patrol giữ nguyên) ...
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Không tìm thấy Waypoint. Dừng Patrol.");
            yield break;
        }

        Debug.Log("Chế độ: TUẦN TRA");
        while (currentState == AIState.Patrol) 
        {
            // ... (Đặt đích Waypoint) ...
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            agent.SetDestination(targetWaypoint.position);
            
            // Chờ đến khi đến nơi HOẶC chuyển trạng thái
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < agent.stoppingDistance || currentState != AIState.Patrol);

            if (currentState != AIState.Patrol) yield break;

            // ... (Logic Đứng chờ) ...
            Debug.Log(gameObject.name + " đã đến đích " + targetWaypoint.name + ". Đứng chờ " + waitTime + " giây.");
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            isWaiting = false;

            // ... (Chuyển điểm tiếp theo) ...
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    IEnumerator ChasePlayer()
    {
        Debug.Log("Chế độ: TRUY ĐUỔI!");
        while (currentState == AIState.Chase)
        {
            if (playerTarget != null)
            {
                // Kiểm tra khoảng cách để chuyển sang Attack
                float distance = Vector3.Distance(transform.position, playerTarget.position);
                
                if (distance <= attackRange)
                {
                    currentState = AIState.Attack; // CHUYỂN SANG ATTACK
                    yield break; 
                }
                
                // Vẫn Chase nếu chưa đủ gần
                agent.SetDestination(playerTarget.position);
            }
            else
            {
                currentState = AIState.Patrol;
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator HandleAttack()
    {
        Debug.Log("Chế độ: TẤN CÔNG!");
        
        while (currentState == AIState.Attack)
        {
            // Kiểm tra Player có còn trong tầm đánh không
            if (playerTarget == null || Vector3.Distance(transform.position, playerTarget.position) > attackRange * 1.5f)
            {
                // Đảm bảo tắt Collider nếu thoát khỏi trạng thái
                if (attackCollider != null) attackCollider.enabled = false; 
                currentState = AIState.Chase; // Chuyển về Chase nếu Player chạy xa
                yield break;
            }
            
            if (!isAttacking)
            {
                isAttacking = true;
                
                // 1. Quay mặt về Player
                Vector3 direction = (playerTarget.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                
                // 2. Kích hoạt Animation Tấn công
                animator.SetBool(isAttackingHash, true);
                
                // =======================================================
                // Logic Tấn công DỰA TRÊN TIMER
                // Chờ một chút để Animation đánh chạy đến khung hình ra đòn (ví dụ: 0.3s)
                yield return new WaitForSeconds(0.3f); 

                // 3. BẬT Collider, Gây sát thương và TẮT Collider ngay lập tức (chỉ 1 frame)
                if (attackCollider != null) attackCollider.enabled = true;
                
                // Chờ 1 frame để OnTriggerEnter được gọi trên Attack Collider
                yield return null; 

                if (attackCollider != null) attackCollider.enabled = false;
                Debug.Log("Đã kích hoạt sát thương dựa trên Timer.");
                
                // 4. Chờ phần còn lại của Animation hoàn thành (Tổng cộng 1.0s)
                yield return new WaitForSeconds(1.0f - 0.3f); // 0.7s còn lại
                
                // 5. Reset Animation và chờ Cooldown
                animator.SetBool(isAttackingHash, false);
                isAttacking = false;
                
                // Chờ thời gian Cooldown
                yield return new WaitForSeconds(attackCooldown);
            }
            yield return null;
        }
    }

    // ... (OnTriggerEnter/Exit giữ nguyên) ...
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerTarget = other.transform;
            if(currentState == AIState.Patrol) currentState = AIState.Chase; 
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = AIState.Patrol; 
            playerTarget = null;
        }
    }
    
    // HÀM NÀY KHÔNG CẦN THIẾT NỮA KHI DÙNG TIMER
    public void EnableAttackCollider() { } 
    public void DisableAttackCollider() { }

    // HÀM NÀY BỊ BỎ QUA VÌ NÊN SỬ DỤNG SCRIPT RIÊNG TRÊN ATTACK COLLIDER
    void OnCollisionEnter(Collision collision) { } 
}