using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;

    [Header("Stats")]
    public float maxHP = 500;
    public float currentHP;

    [Header("Combat Settings")]
    public float detectRange = 15f;
    public float attackRange = 3f;
    public float skill1Range = 10f;
    public float skill2Range = 12f;

    public float attackCooldown = 2f;
    public float skill1Cooldown = 5f;
    public float skill2Cooldown = 8f;

    private float nextAttackTime;
    private float nextSkill1Time;
    private float nextSkill2Time;

    [Header("VFX")]
    public GameObject attackVFX;
    public Transform attackPoint;

    public GameObject skill1VFX;
    public Transform skill1Point;

    public GameObject skill2VFX;
    public Transform skill2Point;

    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // ---- DIE CHECK ----
        if (currentHP <= 0)
        {
            Die();
            return;
        }

        // ---- PLAYER OUT OF RANGE → IDLE ----
        if (distance > detectRange)
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
            return;
        }

        // ---- MOVE TỚI PLAYER ----
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;

            TryAttack();
            TrySkill1(distance);
            TrySkill2(distance);
        }

        FacePlayer();
    }

    void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            animator.SetBool("isAttacking", true);
        }
    }

    void TrySkill1(float dist)
    {
        if (dist <= skill1Range && Time.time >= nextSkill1Time)
        {
            nextSkill1Time = Time.time + skill1Cooldown;
            animator.SetBool("isCasting1", true);
        }
    }

    void TrySkill2(float dist)
    {
        if (dist <= skill2Range && Time.time >= nextSkill2Time)
        {
            nextSkill2Time = Time.time + skill2Cooldown;
            animator.SetBool("isCasting2", true);
        }
    }

    void FacePlayer()
    {
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), 10f * Time.deltaTime);
    }

    // ---------------------------------------------------------
    //  CALLED BY ANIMATION EVENT
    // ---------------------------------------------------------
    public void SpawnAttackVFX()
    {
        Instantiate(attackVFX, attackPoint.position, attackPoint.rotation);
        animator.SetBool("isAttacking", false);
    }

    public void CastSkill1()
    {
        Instantiate(skill1VFX, skill1Point.position, skill1Point.rotation);
        animator.SetBool("isCasting1", false);
    }

    public void CastSkill2()
    {
        Instantiate(skill2VFX, skill2Point.position, skill2Point.rotation);
        animator.SetBool("isCasting2", false);
    }

    // ---------------------------------------------------------
    //  DAMAGE & DIE
    // ---------------------------------------------------------
    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentHP -= dmg;

        if (currentHP <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        agent.isStopped = true;

        Destroy(gameObject, 5f);  // Xóa sau 5s
    }
}
