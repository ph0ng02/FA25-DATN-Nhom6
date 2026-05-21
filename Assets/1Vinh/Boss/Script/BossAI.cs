using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossAI : MonoBehaviour
{
    enum BossState { Idle, Move, Attack, Skill, Dead }
    BossState state = BossState.Idle;

    [Header("References")]
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;

    [Header("Stats")]
    public float maxHP = 500f;
    float currentHP;

    [Header("Ranges")]
    public float detectRange = 20f;
    public float attackRange = 3f;
    public float skillRange = 15f;

    [Header("Damage")]
    public float attackDamage = 20f;
    public float skill1Damage = 35f;
    public float skill2Damage = 50f;

    [Header("Cooldown")]
    public float attackCooldown = 1.2f;
    public float skill1Cooldown = 10f;
    public float skill2Cooldown = 20f;

    float lastAttack;
    float lastSkill1;
    float lastSkill2;

    [Header("VFX")]
    public GameObject skill1Projectile;
    public Transform skill1Point;

    public GameObject skill2Projectile;
    public Transform skill2Point;

    Vector3 lockedPlayerPos;

    bool isAction;

    void Start()
    {
        currentHP = maxHP;
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (state == BossState.Dead || player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (isAction) return;

        if (dist > detectRange)
        {
            SetIdle();
            return;
        }

        if (dist > attackRange)
        {
            MoveToPlayer();
            return;
        }

        FacePlayer();
        DecideCombat();
    }

    // =======================
    // LOGIC
    // =======================

    void DecideCombat()
    {
        float t = Time.time;

        if (t - lastSkill2 >= skill2Cooldown && Vector3.Distance(transform.position, player.position) <= skillRange)
        {
            StartCoroutine(CastSkill2());
            return;
        }

        if (t - lastSkill1 >= skill1Cooldown && Vector3.Distance(transform.position, player.position) <= skillRange)
        {
            StartCoroutine(CastSkill1());
            return;
        }

        if (t - lastAttack >= attackCooldown)
        {
            StartCoroutine(Attack());
        }
    }

    // =======================
    // MOVE / IDLE
    // =======================

    void MoveToPlayer()
    {
        state = BossState.Move;
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void SetIdle()
    {
        state = BossState.Idle;
        agent.isStopped = true;
        animator.SetFloat("Speed", 0);
    }

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
    }

    // =======================
    // ACTIONS
    // =======================

    IEnumerator Attack()
    {
        isAction = true;
        state = BossState.Attack;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.8f);

        lastAttack = Time.time;
        isAction = false;
    }

    IEnumerator CastSkill1()
    {
        isAction = true;
        state = BossState.Skill;

        lockedPlayerPos = player.position;
        animator.SetTrigger("Skill1");

        yield return new WaitForSeconds(1.3f);

        lastSkill1 = Time.time;
        isAction = false;
    }

    IEnumerator CastSkill2()
    {
        isAction = true;
        state = BossState.Skill;

        lockedPlayerPos = player.position;
        animator.SetTrigger("Skill2");

        yield return new WaitForSeconds(1.6f);

        lastSkill2 = Time.time;
        isAction = false;
    }

    // =======================
    // ANIMATION EVENTS
    // =======================

    public void NormalAttackHit()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<HealthManagement>()?.TakeDamage(attackDamage);
        }
    }

    public void Skill1Fire()
    {
        FireProjectile(skill1Projectile, skill1Point, skill1Damage);
    }

    public void Skill2Fire()
    {
        FireProjectile(skill2Projectile, skill2Point, skill2Damage);
    }

    void FireProjectile(GameObject prefab, Transform point, float dmg)
    {
        if (!prefab) return;

        GameObject proj = Instantiate(prefab, point.position, Quaternion.identity);
        proj.transform.LookAt(lockedPlayerPos);

        proj.GetComponent<BossProjectile>().Init(lockedPlayerPos, dmg);
    }

    // =======================
    // DAMAGE
    // =======================

    public void TakeDamage(float dmg)
    {
        if (state == BossState.Dead) return;

        currentHP -= dmg;
        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        state = BossState.Dead;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 5f);
    }
}
