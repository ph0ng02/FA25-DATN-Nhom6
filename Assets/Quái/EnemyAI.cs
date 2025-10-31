using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float patrolRadius = 8f;
    public float patrolWaitTime = 3f;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private Vector3 patrolTarget;
    private bool isChasing;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewPatrolPoint();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= attackRange)
        {
            AttackPlayer();
        }
        else if (distance <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void ChasePlayer()
    {
        isChasing = true;
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position); // Dừng di chuyển
        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Enemy attacks!");
            // Thêm animation hoặc trừ máu người chơi tại đây
            lastAttackTime = Time.time;
        }
    }

    void Patrol()
    {
        if (isChasing)
        {
            isChasing = false;
            SetNewPatrolPoint();
        }

        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            Invoke(nameof(SetNewPatrolPoint), patrolWaitTime);
        }

        agent.SetDestination(patrolTarget);
    }

    void SetNewPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
        patrolTarget = hit.position;
    }
}
