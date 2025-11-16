using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;
    public NavMeshAgent agent;
    public Transform player;

    [Header("Stats")]
    public float detectionRange = 15f;
    public float runFastRange = 12f;
    public float attackRange = 4f;

    public int maxHP = 1000;
    private int currentHP;
    private bool isDead = false;

    private float distance;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        if (isDead) return;

        // tính khoảng cách
        distance = Vector3.Distance(transform.position, player.position);

        // đẩy Distance vào Animator
        anim.SetFloat("Distance", distance);

        // follow player
        agent.SetDestination(player.position);

        // chọn state theo khoảng cách
        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        if (distance > detectionRange)
        {
            // đứng im
            agent.isStopped = true;
            anim.Play("Idle");
        }
        else if (distance > runFastRange)
        {
            // chạy thường
            agent.isStopped = false;
            agent.speed = 3.5f;
            anim.Play("Run");
        }
        else if (distance > attackRange)
        {
            // chạy nhanh hơn
            agent.isStopped = false;
            agent.speed = 5.5f;
            anim.Play("Run 0");
        }
    }

    void HandleAttack()
    {
        if (distance <= attackRange)
        {
            agent.isStopped = true;

            // chọn ngẫu nhiên 3 skill attack
            int atk = Random.Range(1, 4);  // 1,2,3
            anim.SetInteger("AttackIndex", atk);

            switch (atk)
            {
                case 1:
                    anim.Play("Attack");
                    break;
                case 2:
                    anim.Play("Attack2");
                    break;
                case 3:
                    anim.Play("Attack3");
                    break;
            }
        }
    }

    // nhận sát thương
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHP -= dmg;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        anim.SetBool("isDead", true);
        anim.Play("Death");

        agent.isStopped = true;

        Destroy(gameObject, 3f);
    }
}
