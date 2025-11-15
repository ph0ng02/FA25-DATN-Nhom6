using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;
    public NavMeshAgent agent;
    public Transform player;

    [Header("Stats")]
    public float maxHP = 1000;
    public float currentHP;
    private bool isDead = false;

    [Header("Ranges")]
    public float detectionRange = 10f;
    public float attackRange = 4f;

    [Header("Cooldowns")]
    public float skillCD = 3f;
    public float throwCD = 4.5f;
    public float walkCD = 2f;

    private float nextSkill;
    private float nextThrow;
    private float nextWalk;

    private bool isAttacking = false;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        if (isDead) return;

        // ======= DEATH =======
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            agent.isStopped = true;
            anim.SetTrigger("Death");
            return;
        }

        // ======= GET HP % =======
        float hpPercent = (currentHP / maxHP) * 100f;
        float distance = Vector3.Distance(transform.position, player.position);

        // ======= OUT OF RANGE = IDLE =======
        if (distance > detectionRange)
        {
            SetIdle();
            return;
        }

        // ======= MOVEMENT =======
        if (!isAttacking && distance > attackRange && Time.time >= nextWalk)
        {
            nextWalk = Time.time + walkCD;

            anim.SetBool("Idle", false);
            anim.SetTrigger("Walk");

            agent.isStopped = false;
            agent.SetDestination(player.position);
            return;
        }

        // ======= STOP BEFORE ATTACK =======
        agent.isStopped = true;

        // =======================================
        //               CHỈ SKILL (HP > 75%)
        // =======================================
        if (hpPercent > 75f)
        {
            TrySkill();
            return;
        }

        // =======================================
        //           SKILL + THROW (50% - 75%)
        // =======================================
        if (hpPercent > 50f)
        {
            if (TryThrow()) return;
            TrySkill();
            return;
        }

        // =======================================
        //       FULL COMBO (40% - 50%)
        // =======================================
        if (hpPercent > 40f)
        {
            if (TryWalkAttack()) return;
            if (TryThrow()) return;
            if (TrySkill()) return;
            return;
        }

        // =======================================
        //             BERSERK MODE (< 40%)
        // =======================================
        if (TrySkill(0.7f)) return;
        TryThrow(0.8f);
    }

    // =====================================================
    // ============= ACTION FUNCTIONS =======================
    // =====================================================

    void SetIdle()
    {
        agent.isStopped = true;
        anim.SetBool("Idle", true);
    }

    bool TrySkill(float multiplier = 1f)
    {
        if (isAttacking) return false;
        if (Time.time < nextSkill) return false;

        nextSkill = Time.time + skillCD * multiplier;

        anim.SetBool("Idle", false);
        anim.SetTrigger("Skill");

        StartCoroutine(AttackLock(1.2f)); // Lock trong 1.2s
        return true;
    }

    bool TryThrow(float multiplier = 1f)
    {
        if (isAttacking) return false;
        if (Time.time < nextThrow) return false;

        nextThrow = Time.time + throwCD * multiplier;

        anim.SetBool("Idle", false);
        anim.SetTrigger("Throw");

        StartCoroutine(AttackLock(1.0f));
        return true;
    }

    bool TryWalkAttack()
    {
        if (isAttacking) return false;
        if (Time.time < nextWalk) return false;

        nextWalk = Time.time + walkCD;

        anim.SetBool("Idle", false);
        anim.SetTrigger("Walk");

        StartCoroutine(AttackLock(0.5f));
        return true;
    }

    // khóa animation, tránh spam
    System.Collections.IEnumerator AttackLock(float time)
    {
        isAttacking = true;
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
    }
}
