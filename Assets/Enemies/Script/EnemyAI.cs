using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 12f;
    public float attackRange = 2.2f;
    public float loseSightTime = 3f;
    public LayerMask obstaclesMask;
    public bool useFieldOfView = true;
    [Range(0, 360)] public float viewAngle = 120f;

    NavMeshAgent agent;
    Animator anim;
    EnemyCombo combo;

    float lostTimer = 0f;
    bool playerInSight = false;

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
            if (dist > attackRange + 0.1f)
            {
                // chase
                agent.isStopped = false;
                agent.SetDestination(player.position);
                anim.SetBool("IsMoving", true);
                // cancel any ongoing combo if you want or let combo finish
            }
            else
            {
                // in attack range
                agent.isStopped = true;
                anim.SetBool("IsMoving", false);
                // Face player smoothly
                Vector3 dir = (player.position - transform.position);
                dir.y = 0;
                if (dir.sqrMagnitude > 0.01f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(dir.normalized),
                        Time.deltaTime * 8f);
                }
                // start combo if not already
                if (!combo.IsInCombo)
                {
                    combo.StartComboSequence(player);
                }
            }
        }
        else
        {
            // lost sight
            lostTimer += Time.deltaTime;
            if (lostTimer < loseSightTime)
            {
                // still chase to last known position (optional)
                agent.isStopped = false;
                agent.SetDestination(player.position);
                anim.SetBool("IsMoving", true);
            }
            else
            {
                // go idle / patrol (not implemented) 
                agent.isStopped = true;
                anim.SetBool("IsMoving", false);
            }
        }

        // update animator speed param (optional)
        anim.SetFloat("MoveSpeed", agent.velocity.magnitude);
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

        // raycast to check obstacles
        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 dir = (player.position + Vector3.up * 1.0f) - origin;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir.normalized, out hit, detectionRadius))
        {
            if (hit.collider.transform == player) return true;
            // optionally check if hit something tagged "Player"
            if (hit.collider.CompareTag("Player")) return true;
            return false;
        }

        return false;
    }

    // debug drawing
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