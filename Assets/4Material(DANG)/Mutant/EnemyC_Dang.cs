using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyC_Dang : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private int isWalkingHash;
    private int isAttackingHash;

    public enum AIState { Patrol, Chase, Attack }
    public AIState currentState = AIState.Patrol;

    [Header("Patrol Settings")]
    public Transform patrolRouteParent;
    public float waitTime = 3f;

    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private bool isWaiting = false;
    private bool isAttacking = false;
    private Transform playerTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            isWalkingHash = Animator.StringToHash("IsWalking");
            isAttackingHash = Animator.StringToHash("IsAttacking");
        }

        if (patrolRouteParent != null)
        {
            waypoints = new Transform[patrolRouteParent.childCount];
            for (int i = 0; i < patrolRouteParent.childCount; i++)
                waypoints[i] = patrolRouteParent.GetChild(i);
        }

        StartCoroutine(FSM());
    }

    void Update()
    {
        if (agent != null)
            agent.isStopped = (isWaiting && currentState == AIState.Patrol) ||
                              currentState == AIState.Attack;

        if (animator != null && agent != null)
        {
            bool isMoving = agent.velocity.magnitude > 0.1f &&
                            !isWaiting &&
                            currentState != AIState.Attack;

            animator.SetBool(isWalkingHash, isMoving);
        }
    }

    IEnumerator FSM()
    {
        while (true)
        {
            if (playerTarget == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) playerTarget = p.transform;
            }

            switch (currentState)
            {
                case AIState.Patrol:
                    yield return PatrolCycle();
                    break;

                case AIState.Chase:
                    yield return ChasePlayer();
                    break;

                case AIState.Attack:
                    yield return HandleAttack();
                    break;
            }

            yield return null;
        }
    }

    IEnumerator PatrolCycle()
    {
        if (waypoints == null || waypoints.Length == 0)
            yield break;

        while (currentState == AIState.Patrol)
        {
            Transform wp = waypoints[currentWaypointIndex];
            agent.SetDestination(wp.position);

            yield return new WaitUntil(() =>
                !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            isWaiting = false;

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            // Gặp player thì chase
            if (playerTarget != null)
            {
                currentState = AIState.Chase;
                yield break;
            }
        }
    }

    IEnumerator ChasePlayer()
    {
        while (currentState == AIState.Chase)
        {
            if (playerTarget == null)
            {
                currentState = AIState.Patrol;
                yield break;
            }

            float distance = Vector3.Distance(transform.position, playerTarget.position);
            if (distance <= attackRange)
            {
                currentState = AIState.Attack;
                yield break;
            }

            agent.SetDestination(playerTarget.position);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator HandleAttack()
    {
        while (currentState == AIState.Attack)
        {
            if (playerTarget == null)
            {
                currentState = AIState.Patrol;
                yield break;
            }

            float dist = Vector3.Distance(transform.position, playerTarget.position);
            if (dist > attackRange * 1.5f)
            {
                currentState = AIState.Chase;
                yield break;
            }

            if (!isAttacking)
            {
                isAttacking = true;

                // Quay mặt về player
                Vector3 dir = (playerTarget.position - transform.position).normalized;
                Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                transform.rotation = lookRot;

                // Attack animation
                animator.SetBool(isAttackingHash, true);

                // Thời gian vung tay
                yield return new WaitForSeconds(0.7f);

                animator.SetBool(isAttackingHash, false);
                isAttacking = false;

                yield return new WaitForSeconds(attackCooldown);
            }

            yield return null;
        }
    }
}
