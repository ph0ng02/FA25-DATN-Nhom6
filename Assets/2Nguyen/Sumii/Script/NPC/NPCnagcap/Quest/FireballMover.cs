using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireballMover : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    [Header("Target & Movement")]
    public Transform questTarget;       // NPC hoặc vị trí nhiệm vụ
    public float moveSpeed = 15f;       // tốc độ bay
    public float followDistance = 3f;   // đi trước Player
    public float stopDistance = 2f;     // dừng khi gần mục tiêu
    public float playerInfluenceRadius = 10f; // phạm vi để Fireball “dẫn đường” theo player

    [Header("Hover Settings")]
    public float upDownSpeed = 0.5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        agent = GetComponent<NavMeshAgent>();
        if (agent == null) agent = gameObject.AddComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = 0;
    }

    private void Update()
    {
        if (player == null || questTarget == null) return;

        // Hover VFX
        agent.baseOffset += upDownSpeed * Time.deltaTime;
        if (agent.baseOffset >= 2f) upDownSpeed = -0.5f;
        else if (agent.baseOffset <= 1.5f) upDownSpeed = 0.5f;

        float distToTarget = Vector3.Distance(transform.position, questTarget.position);

        // Nếu tới gần target thì hủy
        if (distToTarget <= stopDistance)
        {
            Destroy(gameObject);
            return;
        }

        // Nếu player ở gần → dẫn đường bằng cách “chạy trước mặt player về phía target”
        float distPlayerToFireball = Vector3.Distance(player.position, transform.position);
        if (distPlayerToFireball <= playerInfluenceRadius)
        {
            Vector3 dirToTarget = (questTarget.position - player.position).normalized;
            Vector3 aheadPos = player.position + dirToTarget * followDistance;

            agent.isStopped = false;
            agent.SetDestination(aheadPos);
        }
        else
        {
            // Player đi xa → Fireball tự đi thẳng tới questTarget
            agent.isStopped = false;
            agent.SetDestination(questTarget.position);
        }
    }
}
