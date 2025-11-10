using UnityEngine;
using UnityEngine.AI;

public class TheOnlyOneBoss : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;
    public NavMeshAgent agent;
    public Transform player;

    [Header("Stats")]
    public int maxHP = 1000;
    private int currentHP;
    private bool isDead = false;

    [Header("Ranges")]
    public float attackRange = 2f;
    public float throwRange = 6f;
    public float skillRange = 10f;

    [Header("Cooldowns")]
    public float attackCooldown = 2f;
    public float throwCooldown = 4f;
    public float skillCooldown = 8f;

    private float nextAttackTime;
    private float nextThrowTime;
    private float nextSkillTime;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Di chuyển hoặc đứng yên
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("isMoving", true);
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("isMoving", false);
        }

        // Gọi hàm tấn công theo giai đoạn máu
        HandlePhases(distance);
    }

    void HandlePhases(float distance)
    {
        float hpPercent = (float)currentHP / maxHP * 100f;

        // 🩸 Giai đoạn 1: > 75% HP → chỉ Attack
        if (hpPercent > 75f)
        {
            if (distance <= attackRange && Time.time >= nextAttackTime)
                Attack();
        }

        // ⚔️ Giai đoạn 2: 50% < HP ≤ 75% → Attack + Throw
        else if (hpPercent > 50f)
        {
            if (distance <= attackRange && Time.time >= nextAttackTime)
                Attack();
            else if (distance <= throwRange && Time.time >= nextThrowTime)
                Throw();
        }

        // 💀 Giai đoạn 3: HP ≤ 40% → dùng hết chiêu (Attack + Throw + Skill)
        else if (hpPercent <= 40f)
        {
            if (distance <= attackRange && Time.time >= nextAttackTime)
                Attack();
            else if (distance <= throwRange && Time.time >= nextThrowTime)
                Throw();
            else if (distance <= skillRange && Time.time >= nextSkillTime)
                Skill();
        }
    }

    void Attack()
    {
        anim.SetTrigger("attackTrigger");
        nextAttackTime = Time.time + attackCooldown;
        Debug.Log("Boss Attack!");
    }

    void Throw()
    {
        anim.SetTrigger("throwTrigger");
        nextThrowTime = Time.time + throwCooldown;
        Debug.Log("Boss Throw!");
    }

    void Skill()
    {
        anim.SetTrigger("useSkill");
        nextSkillTime = Time.time + skillCooldown;
        Debug.Log("Boss Skill!");
    }

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
        agent.isStopped = true;
        anim.SetBool("isDead", true);
        Debug.Log("Boss Dead!");
        Destroy(gameObject, 5f);
    }

    // Gọi bằng Animation Event
    public void DealDamage()
    {
        Debug.Log("Boss hits player!");
    }

    public void SpawnProjectile()
    {
        Debug.Log("Boss throws projectile!");
    }
}
