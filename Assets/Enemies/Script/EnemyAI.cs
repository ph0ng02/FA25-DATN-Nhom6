using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyCombo))]
public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform player;
    public float detectionRadius = 10f;
    public float attackRange = 2.3f;
    public float loseSightTime = 3f;
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
            float dist = Vector3.Distance(transform.position, player.position);

            if (dist > attackRange)
            {
                // Chase player
                agent.isStopped = false;
                agent.SetDestination(player.position);
                anim.SetBool(moveBool, true);
            }
            else
            {
                // Attack player
                agent.isStopped = true;
                anim.SetBool(moveBool, false);

                Vector3 dir = (player.position - transform.position);
                dir.y = 0;
                if (dir.sqrMagnitude > 0.01f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(dir.normalized),
                        Time.deltaTime * 10f);
                }

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
            lostTimer += Time.deltaTime;
            if (lostTimer < loseSightTime)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                anim.SetBool(moveBool, true);
            }
            else
            {
                agent.isStopped = true;
                anim.SetBool(moveBool, false);
            }
        }

        anim.SetFloat(speedFloat, agent.velocity.magnitude);
    }

    bool CheckPlayerInSight()
    {
        Vector3 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;
        if (dist > detectionRadius) return false;

        if (useFieldOfView)
        {
            float angle = Vector3.Angle(transform.forward, toPlayer);
            if (angle > viewAngle / 2f) return false;
        }

        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 dir = (player.position + Vector3.up * 1.0f) - origin;
        if (Physics.Raycast(origin, dir.normalized, out RaycastHit hit, detectionRadius))
        {
            if (hit.collider.transform == player || hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }
}