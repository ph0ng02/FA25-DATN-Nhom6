using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;

    [Header("Stats")]
    public float maxHP = 500f;
    private float currentHP;

    [Header("Ranges")]
    public float detectRange = 15f;
    public float attackRange = 3f;

    [Header("Damage")]
    public float attackDamage = 20f;
    public float skill1Damage = 35f;
    public float skill2Damage = 50f;

    [Header("Cooldowns")]
    public float attackCooldown = 1f;

    public float skill1Start = 10f;
    public float skill1Cooldown = 20f;

    public float skill2Start = 30f;
    public float skill2Cooldown = 30f;

    float lastAttack = -999f;
    float lastSkill1 = -999f;
    float lastSkill2 = -999f;

    bool isAction = false;
    bool isDead = false;

    [Header("VFX")]
    public GameObject skill1VFX;
    public Transform skill1Point;

    public GameObject skill2VFX;
    public Transform skill2Point;

    private void Start()
    {
        currentHP = maxHP;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Di chuyển theo player nếu không đang cast
        if (!isAction && dist <= detectRange && dist > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
        }

        // Trong tầm đánh
        if (dist <= attackRange)
        {
            FacePlayer();

            if (!isAction)
                CombatLogic();
        }
    }

    private void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * 10f
        );
    }

    private void CombatLogic()
    {
        float t = Time.time;

        // Skill 2 — Ưu tiên cao nhất
        if (t >= skill2Start && t - lastSkill2 >= skill2Cooldown)
        {
            StartCoroutine(DoSkill2());
            return;
        }

        // Skill 1
        if (t >= skill1Start && t - lastSkill1 >= skill1Cooldown)
        {
            StartCoroutine(DoSkill1());
            return;
        }

        // Đánh thường
        if (t - lastAttack >= attackCooldown)
        {
            StartCoroutine(DoAttack());
            return;
        }
    }

    // ===========================
    // ACTIONS
    // ===========================

    IEnumerator DoAttack()
    {
        isAction = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.7f);

        lastAttack = Time.time;   // cooldown đặt SAU khi đánh xong
        isAction = false;
    }

    IEnumerator DoSkill1()
    {
        isAction = true;
        animator.SetTrigger("Skill1");

        yield return new WaitForSeconds(1.2f);

        lastSkill1 = Time.time;   // cooldown đặt SAU khi skill xong
        isAction = false;
    }

    IEnumerator DoSkill2()
    {
        isAction = true;
        animator.SetTrigger("Skill2");

        yield return new WaitForSeconds(1.5f);

        lastSkill2 = Time.time;   // cooldown đặt SAU khi skill xong
        isAction = false;
    }

    // ===========================
    // ANIMATION EVENTS
    // ===========================

    public void NormalAttack()
    {
        DealDamage(attackDamage);
    }

    public void Skill1Hit()
    {
        DealDamage(skill1Damage);
        if (skill1VFX != null)
            Instantiate(skill1VFX, skill1Point.position, skill1Point.rotation);
    }

    public void Skill2Hit()
    {
        DealDamage(skill2Damage);
        if (skill2VFX != null)
            Instantiate(skill2VFX, skill2Point.position, skill2Point.rotation);
    }

    // ===========================
    // DAMAGE
    // ===========================

    void DealDamage(float dmg)
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange + 0.5f)
        {
            HealthManagement hp = player.GetComponent<HealthManagement>();
            if (hp != null)
                hp.TakeDamage(dmg);
        }
    }

    // ===========================
    // BOSS TAKE DAMAGE
    // ===========================
    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentHP -= dmg;

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 5f);
    }
}
