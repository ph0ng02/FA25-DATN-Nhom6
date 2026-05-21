using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollState_Dragon : StateMachineBehaviour
{
    float timer;
    List<Transform> Waypoint_Enemy2 = new List<Transform>();
    NavMeshAgent agent;

    Transform player;
    float chaseRange = 100f;

    // Khi bắt đầu vào trạng thái Patrolling
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 8f;
        timer = 0f;

        // Lấy tất cả waypoint có tag "Waypoint_Enemy2"
        GameObject[] goArray = GameObject.FindGameObjectsWithTag("Waypoint_Enemy2");

        // Xóa danh sách cũ
        Waypoint_Enemy2.Clear();

        // Thêm tất cả Transform vào danh sách
        foreach (GameObject go in goArray)
        {
            Waypoint_Enemy2.Add(go.transform);
        }

        // Chỉ set điểm đến nếu có waypoint
        if (Waypoint_Enemy2.Count > 0)
        {
            int randomIndex = Random.Range(0, Waypoint_Enemy2.Count);
            agent.SetDestination(Waypoint_Enemy2[randomIndex].position);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy waypoint nào với tag 'Waypoint_Enemy2'.");
        }
    }

    // Khi đang ở trạng thái Patrolling
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Waypoint_Enemy2.Count > 0)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                int randomIndex = Random.Range(0, Waypoint_Enemy2.Count);
                agent.SetDestination(Waypoint_Enemy2[randomIndex].position);
            }
        }

        timer += Time.deltaTime;
        if (timer > 5f)
            animator.SetBool("IsPatrolling", false);

        if (player != null)
        {
            float distance = Vector3.Distance(player.position, animator.transform.position);
            if (distance < chaseRange)
                animator.SetBool("IsChasing", true);
        }
    }

    // Khi thoát khỏi trạng thái Patrolling
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
            agent.SetDestination(agent.transform.position);
    }
}
