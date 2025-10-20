using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyCombo))]
public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform player;
    public float detectionRadius = 12f;
    public float attackRange = 2.2f;
    public float loseSightTime = 3f;
    public LayerMask obstaclesMask;
    public bool useFieldOfView = true;
    [Range(0, 360)] public float viewAngle = 120f;

    [Header("Animation Settings")]
    public string moveBool = "IsMoving";
    public string speedFloat = "MoveSpeed";
    public string attackTrigger = "AttackTrigger";
    public string attackIndexInt = "AttackIndex";

    private NavMeshAgent agent;
    private Animator anim;
    private EnemyCombo combo;
    private float lostTimer = 0f;
    private bool playerInSight = false;

    // Dùng để lưu vận tốc mượt
    private float smoothSpeed;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        combo = GetComponent<EnemyCombo>();

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        playerInSight = CheckPlayerInSight();

        if (playerInSight)
        {
            lostTimer = 0f;
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackRange)
            {
                // --- DI CHUYỂN ---
                agent.isStopped = false;
                agent.SetDestination(player.position);

                // Blend tốc độ để tránh giật
                smoothSpeed = Mathf.Lerp(smoothSpeed, agent.velocity.magnitude, Time.deltaTime * 8f);
                anim.SetFloat(speedFloat, smoothSpeed);
                anim.SetBool(moveBool, smoothSpeed > 0.1f);
            }
            else
            {
                // --- TẤN CÔNG ---
                agent.isStopped = true;
                anim.SetBool(moveBool, false);
                anim.SetFloat(speedFloat, 0);

                FacePlayer();

                if (!combo.IsInCombo)
                {
                    int attackIndex = combo.StartComboSequence(player);
                    anim.SetInteger(attackIndexInt, attackIndex);
                    anim.SetTrigger(attackTrigger);
                }
            }
        }
        else
        {
            // --- MẤT TẦM NHÌN ---
            lostTimer += Time.deltaTime;
            if (lostTimer < loseSightTime)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else
            {
                agent.isStopped = true;
            }

            // Blend Idle
            smoothSpeed = Mathf.Lerp(smoothSpeed, agent.velocity.magnitude, Time.deltaTime * 8f);
            anim.SetFloat(speedFloat, smoothSpeed);
            anim.SetBool(moveBool, smoothSpeed > 0.1f);
        }
    }

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 8f);
        }
    }

    bool CheckPlayerInSight()
    {
        Vector3 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;
        if (dist > detectionRadius) return false;

        if (useFieldOfView)
        {
            float angle = Vector3.Angle(transform.forward, toPlayer);
            if (angle > viewAngle * 0.5f) return false;
        }

        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 dir = (player.position + Vector3.up * 1f) - origin;
        if (Physics.Raycast(origin, dir.normalized, out RaycastHit hit, detectionRadius))
        {
            if (hit.collider.transform == player || hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (useFieldOfView)
        {
            Vector3 left = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
            Vector3 right = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + left * detectionRadius);
            Gizmos.DrawLine(transform.position, transform.position + right * detectionRadius);
        }
    }
}