using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class WanderAI : MonoBehaviour
{
    private NavMeshAgent agent; 
    private Animator animator;
    private int isWalkingHash; 

    // --- Cấu hình AI ---
    [Header("AI Settings")]
    [Tooltip("Thời gian đứng yên (giây) khi đến đích")]
    public float waitTime = 3f; 
    
    [Tooltip("Bán kính tìm kiếm điểm ngẫu nhiên xung quanh vị trí hiện tại")]
    public float wanderRadius = 15f; 
    
    [Header("Map Boundary Settings")]
    [Tooltip("Tâm của khu vực hoạt động (Map Center)")]
    public Vector3 mapCenter = Vector3.zero;
    [Tooltip("Bán kính tối đa được phép di chuyển trong map")]
    public float mapRadius = 30f; 

    private bool isWaiting = false; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogError("Lỗi: NavMeshAgent không tìm thấy trên " + gameObject.name);
            return;
        }
        
        // Tối ưu hóa: Lấy Hash của tham số Animator (giúp tránh lỗi nếu tham số không tồn tại)
        if (animator != null)
        {
            isWalkingHash = Animator.StringToHash("IsWalking");
        }

        // Bắt đầu chu trình AI
        StartCoroutine(WanderCycle());
    }

        void Update()
    {
        // Kiểm soát việc dừng/chạy của NavMesh Agent khi chờ
        agent.isStopped = isWaiting;

        // --- Điều khiển Animation ---
        if (animator != null)
        {
            bool isMoving = agent.velocity.magnitude > 0.1f && !isWaiting;
        
            // SỬA LỖI: Sử dụng try-catch block thay vì HasParameter() để tương thích với Unity cũ hơn
            try
            {
                animator.SetBool(isWalkingHash, isMoving); 
            }
            catch (System.Exception e)
            {
                // Bỏ qua lỗi nếu tham số không tồn tại.
                // Có thể bỏ qua e.Message hoặc Debug.Log nếu muốn
            }
        }
    }

    IEnumerator WanderCycle()
    {
        while (true) 
        {
            // 1. CHỜ KHI ĐÃ ĐẾN ĐÍCH
            Debug.Log(gameObject.name + " đã đến đích. Đứng chờ " + waitTime + " giây.");
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            isWaiting = false;

            // 2. TÌM ĐIỂM ĐẾN MỚI
            Vector3 newTarget = Vector3.zero;
            bool targetFound = false;
            int attemptCount = 0;
            const int maxAttempts = 10;

            // Thử tìm điểm ngẫu nhiên cho đến khi tìm được điểm hợp lệ hoặc hết số lần thử
            while (!targetFound && attemptCount < maxAttempts)
            {
                // Tìm kiếm xung quanh vị trí HIỆN TẠI của nhân vật
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += transform.position;

                NavMeshHit hit;
                // Kiểm tra 2 điều kiện: 
                // a) Điểm đó nằm trên NavMesh, và b) Điểm đó nằm trong giới hạn MapRadius
                if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    if (Vector3.Distance(hit.position, mapCenter) <= mapRadius)
                    {
                        newTarget = hit.position;
                        agent.SetDestination(newTarget);
                        Debug.Log("Điểm đích mới được tìm thấy và nằm trong giới hạn: " + newTarget);
                        targetFound = true;
                    }
                }

                attemptCount++;
                if (!targetFound)
                {
                    // Nếu không tìm được điểm, đợi 1 frame trước khi thử lại
                    yield return null; 
                }
            }

            if (!targetFound)
            {
                Debug.LogWarning("Không tìm được điểm đích hợp lệ sau " + maxAttempts + " lần thử. Đứng yên.");
                // Nếu không tìm được, đợi thêm 1 giây rồi thử lại chu trình
                yield return new WaitForSeconds(1f);
                continue; 
            }

            // 3. CHỜ ĐẾN KHI ĐI ĐẾN NƠI
            // Dùng 90% Stopping Distance để Agent không dừng quá xa
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < agent.stoppingDistance * 0.9f);
        }
    }
}