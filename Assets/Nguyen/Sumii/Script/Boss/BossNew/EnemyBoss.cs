using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : MonoBehaviour
{
    [Header("Component")]
    public Animator anim;
    public NavMeshAgent agent;
    public Transform target;

    [Header("Stats")]
    public float detectRange = 10f;
    public float attackRange = 3f;
    public bool dead = false;

    float attackCooldown = 2f;
    float attackTimer = 0f;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (dead) return;

        attackTimer -= Time.deltaTime;

        float dist = Vector3.Distance(transform.position, target.position);

        // -------------------- DI CHUYỂN --------------------
        if (dist > attackRange && dist < detectRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);

            anim.SetBool("isMoving", true);
            anim.SetFloat("speed", agent.velocity.magnitude);
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("isMoving", false);
            anim.SetFloat("speed", 0);
        }

        // -------------------- TẤN CÔNG --------------------
        if (dist <= attackRange && attackTimer <= 0)
        {
            attackTimer = attackCooldown;

            int randAttack = Random.Range(0, 2); // 0 = Throw, 1 = Skill
            anim.SetInteger("attackTyp", randAttack);

            if (randAttack == 0)
            {
                anim.SetTrigger("throwTrigger");
            }
            else
            {
                anim.SetTrigger("useSkill");
            }
        }
    }

    // -------------------- CHẾT --------------------
    public void Die()
    {
        dead = true;
        agent.isStopped = true;

        anim.SetBool("isDead", true);
    }
}
