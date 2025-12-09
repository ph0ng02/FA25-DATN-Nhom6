using UnityEngine;

public class EnemyCastSkill : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;
    public GameObject projectilePrefab;
    public Transform castPoint;   // vị trí tạo quả cầu

    [Header("Skill Settings")]
    public float detectRange = 10f;
    public float timeBetweenCasts = 3f;
    private float lastCastTime = 0f;

    private void Start()
    {
        // 🔥 Tự động tìm player trong scene
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        // TẮT ROOT MOTION để enemy KHÔNG TỰ DI CHUYỂN
        if (animator != null)
            animator.applyRootMotion = false;

        // FREEZE rigidbody nếu có
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // TẮT NavMeshAgent nếu có
        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
        {
            agent.enabled = false;
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Nếu player nằm trong range thì cast
        if (distance <= detectRange)
        {
            if (Time.time - lastCastTime >= timeBetweenCasts)
            {
                LookAtPlayer();
                animator.SetTrigger("Cast");
                lastCastTime = Time.time;
            }
        }
    }

    public void SpawnProjectile()
    {
        GameObject obj = Instantiate(projectilePrefab, castPoint.position, castPoint.rotation);

        MagicProjectile proj = obj.GetComponent<MagicProjectile>();
        if (proj != null)
        {
            proj.SetTarget(player);  // skill tự dí player
        }
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;

        // Nếu quá gần thì không xoay
        if (dir.magnitude < 1f) return;

        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
