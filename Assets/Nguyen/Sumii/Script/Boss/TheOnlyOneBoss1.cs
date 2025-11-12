using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TheOnlyOneBoss1 : MonoBehaviour
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

    [Header("Skill 3 AOE")]
    public GameObject chargeEffectPrefab;
    public GameObject skillEffectPrefab;
    public Transform chargePoint;
    public float skillChargeTime = 2f;
    public int skillDamage = 100;
    public float skillAOERadius = 5f;

    private GameObject currentChargeEffect;

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

        // Xoay mặt về phía player
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), 5f * Time.deltaTime);

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

        HandlePhases(distance);
    }

    void HandlePhases(float distance)
    {
        float hpPercent = (float)currentHP / maxHP * 100f;

        if (hpPercent > 75f)
        {
            if (distance <= attackRange && Time.time >= nextAttackTime)
                Attack();
        }
        else if (hpPercent > 50f)
        {
            if (distance <= attackRange && Time.time >= nextAttackTime)
                Attack();
            else if (distance <= throwRange && Time.time >= nextThrowTime)
                Throw();
        }
        else if (hpPercent <= 50f)
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
        Debug.Log("Boss starts charging AOE skill!");
        agent.isStopped = true;

        if (chargeEffectPrefab && chargePoint)
            currentChargeEffect = Instantiate(chargeEffectPrefab, chargePoint.position, chargePoint.rotation, chargePoint);

        StartCoroutine(CastAOESkillAfterCharge());
    }

    private IEnumerator CastAOESkillAfterCharge()
    {
        yield return new WaitForSeconds(skillChargeTime);

        if (currentChargeEffect != null)
            Destroy(currentChargeEffect);

        if (skillEffectPrefab && chargePoint)
            Instantiate(skillEffectPrefab, chargePoint.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(chargePoint.position, skillAOERadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<PlayerHealth>()?.TakeDamage(skillDamage);
        }

        yield return new WaitForSeconds(0.5f); // nghỉ 0.5s
        agent.isStopped = false;
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;
        currentHP -= dmg;
        if (currentHP <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        anim.SetBool("isDead", true);
        Debug.Log("Boss Dead!");
        Destroy(gameObject, 5f);
    }

    public void DealDamage() => Debug.Log("Boss hits player!");
    public void SpawnProjectile() => Debug.Log("Boss throws projectile!");

    void OnDrawGizmosSelected()
    {
        if (chargePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(chargePoint.position, skillAOERadius);
        }
    }
}
