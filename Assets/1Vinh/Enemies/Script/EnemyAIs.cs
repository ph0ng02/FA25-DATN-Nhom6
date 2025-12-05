using UnityEngine;
using UnityEngine.AI;

public class EnemyAIs : MonoBehaviour
{
    public NavMeshAgent agent;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float attackTimer = 0f;

    private Transform player;
    private Animator anim;

    private bool canSeePlayer = false; // player đang ở trong vùng tầm nhìn

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    public void PlayerInVision(bool state)
    {
        canSeePlayer = state;

        if (!state)
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);
        }
    }

    private void Update()
    {
        if (!canSeePlayer) return;  // ❗ Không thấy player → đứng yên

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("Walk", true);
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("Walk", false);

            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                anim.SetTrigger("Attack");
                attackTimer = attackCooldown;
            }
        }
    }
}
