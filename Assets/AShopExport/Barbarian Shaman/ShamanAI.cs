using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ShamanAI : MonoBehaviour
{
    private NavMeshAgent agent; 
    private Animator animator;
    
    // Giữ nguyên các trạng thái, nhưng hành vi sẽ khác
    public enum AIState { Idle, CastSpell } // Đơn giản hóa thành Idle (chờ) và CastSpell
    public AIState currentState = AIState.Idle; 

    private int isCastingHash; 

    [Header("Target & Cast Settings")]
    public Transform playerTarget; 
    [Tooltip("Khoảng cách tối ưu để Shaman dừng lại niệm phép")]
    public float castRange = 8f; 
    public float castTime = 1.5f; 
    public float castCooldown = 3f; 
    
    public GameObject spellPrefab; 
    public Transform spellOrigin; 

    private bool isCasting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Loại bỏ NavMeshAgent nếu không cần di chuyển, nhưng giữ lại để không bị lỗi.
        // Chỉ cần đảm bảo nó luôn dừng.
        if (agent != null) agent.isStopped = true;

        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true; 
        }

        if (animator != null)
        {
            // Chúng ta không dùng IsWalking nữa
            isCastingHash = Animator.StringToHash("IsCasting"); 
        }
        
        // Bắt đầu vòng lặp AI
        StartCoroutine(FSM()); 
    }

    void Update()
    {
        // Shaman luôn dừng
        if (agent != null) agent.isStopped = true;

        // Đảm bảo Animation về Idle nếu không Niệm phép
        if (animator != null)
        {
            // Nếu bạn có IsWalking, đặt nó về false
            // try { animator.SetBool(Animator.StringToHash("IsWalking"), false); } catch {}
        }
    }

    IEnumerator FSM()
    {
        while (true)
        {
            switch (currentState)
            {
                case AIState.Idle:
                    yield return StartCoroutine(HandleIdle());
                    break;
                case AIState.CastSpell: 
                    yield return StartCoroutine(HandleCastSpell());
                    break;
            }
            yield return null; 
        }
    }
    
    IEnumerator HandleIdle()
    {
        Debug.Log("Chế độ: ĐỨNG CHỜ");
        while (currentState == AIState.Idle) 
        {
            // Shaman chỉ chờ cho đến khi tìm thấy Player (qua OnTriggerEnter)
            if (playerTarget != null)
            {
                currentState = AIState.CastSpell;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator HandleCastSpell()
    {
        Debug.Log("Chế độ: NIỆM PHÉP!");
        
        while (currentState == AIState.CastSpell)
        {
            if (playerTarget == null)
            {
                currentState = AIState.Idle; // Quay về Idle nếu mất mục tiêu
                yield break;
            }
            
            float distance = Vector3.Distance(transform.position, playerTarget.position);

            if (distance > castRange)
            {
                // Nếu Player chạy ra khỏi tầm, Shaman không di chuyển, chỉ quay về Idle
                currentState = AIState.Idle; 
                yield break;
            }
            
            if (!isCasting)
            {
                isCasting = true;
                
                // 1. Quay mặt về Player
                Vector3 direction = (playerTarget.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
                
                // 2. Kích hoạt Animation Niệm phép/Attack
                animator.SetBool(isCastingHash, true);
                
                // 3. Chờ thời gian Niệm phép
                yield return new WaitForSeconds(castTime); 
                
                // 4. Kích hoạt Phép thuật (Spawn Projectile)
                CastProjectile(); 

                // 5. Kết thúc Animation và chờ Cooldown
                animator.SetBool(isCastingHash, false);
                isCasting = false;
                
                yield return new WaitForSeconds(castCooldown);
            }
            yield return null;
        }
    }
    
    void CastProjectile()
    {
        if (spellPrefab != null && spellOrigin != null && playerTarget != null)
        {
            // Tạo ra Projectile (đạn phép)
            GameObject spell = Instantiate(spellPrefab, spellOrigin.position, transform.rotation);
            
            // GỌI HÀM FIRE VÀ TRUYỀN VỊ TRÍ PLAYER
            Vector3 targetPos = playerTarget.position + Vector3.up * 1f; 
            spell.GetComponent<SpellProjectile>().Fire(targetPos);

            Debug.Log("Phù thủy đã bắn Phép thuật!");
        }
    }

    // Hàm cảm biến khi Player đi vào vùng nhận diện (Sphere Collider)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerTarget = other.transform; 
            currentState = AIState.CastSpell; // Bắt đầu tấn công ngay lập tức
        }
    }
    
    // Hàm cảm biến khi Player rời khỏi vùng nhận diện
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = null;
            currentState = AIState.Idle; 
        }
    }
}