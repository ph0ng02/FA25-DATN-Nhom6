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
    public GameObject chargeEffectPrefab;   // Hiệu ứng charge
    public GameObject skillEffectPrefab;    // Hiệu ứng skill thật (AOE nổ)
    public Transform chargePoint;           // Vị trí xuất hiện charge
    public float skillChargeTime = 2f;      // Thời gian mém chiêu
    public int skillDamage = 100;           // ✅ Đã đổi sang int
    public float skillAOERadius = 5f;       // Bán kính AOE skill

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

        // Gọi hành vi theo giai đoạn máu
        HandlePhases(distance);
    }

    void HandlePhases(float distance)
    {
        float hpPercent = (float)currentHP / maxHP * 100f;

        // 🩸 Giai đoạn 1: >75% HP → chỉ Attack
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
        Debug.Log("Boss starts charging AOE skill!");

        // Hiện hiệu ứng charge
        if (chargeEffectPrefab != null && chargePoint != null)
        {
            currentChargeEffect = Instantiate(chargeEffectPrefab, chargePoint.position, chargePoint.rotation, chargePoint);
        }

        // Bắt đầu Coroutine thực hiện skill sau khi charge
        StartCoroutine(CastAOESkillAfterCharge());
    }

    private IEnumerator CastAOESkillAfterCharge()
    {
        // Boss đứng yên khi charge
        agent.isStopped = true;
        anim.SetBool("isMoving", false);

        // Chờ thời gian charge
        yield return new WaitForSeconds(skillChargeTime);

        // Xóa hiệu ứng charge
        if (currentChargeEffect != null)
            Destroy(currentChargeEffect);

        // Hiện hiệu ứng skill thật
        if (skillEffectPrefab != null && chargePoint != null)
        {
            Instantiate(skillEffectPrefab, chargePoint.position, Quaternion.identity);
        }

        // Gây damage cho player trong bán kính AOE
        Collider[] hits = Physics.OverlapSphere(chargePoint.position, skillAOERadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerHealth>()?.TakeDamage(skillDamage); // ✅ Ok vì skillDamage là int
            }
        }

        // Boss có thể di chuyển lại
        agent.isStopped = false;
    }

    public void TakeDamage(int dmg)
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
        anim.SetBool("isDead", true);
        Debug.Log("Boss Dead!");
        Destroy(gameObject, 5f);
    }

    // Animation Event: DealDamage (Attack)
    public void DealDamage()
    {
        Debug.Log("Boss hits player!");
    }

    // Animation Event: SpawnProjectile (Throw)
    public void SpawnProjectile()
    {
        Debug.Log("Boss throws projectile!");
    }

    // Vẽ vùng AOE trong Scene để test
    void OnDrawGizmosSelected()
    {
        if (chargePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(chargePoint.position, skillAOERadius);
        }
    }
}
