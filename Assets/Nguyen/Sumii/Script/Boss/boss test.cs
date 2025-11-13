using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class bosstest : MonoBehaviour
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

    [Header("FX Settings")]
    public GameObject fxCirclePrefab;
    public float fxRotateSpeed = 50f;
    public float fxPulseSpeed = 2f;
    public float fxMinIntensity = 0.5f;
    public float fxMaxIntensity = 1.2f;

    private GameObject fxCircleInstance;
    private Material fxMaterial;
    private float fxPulseTime;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        currentHP = maxHP;

        agent.stoppingDistance = attackRange - 0.3f;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.avoidancePriority = 50;

        // 🌀 Spawn FX dưới chân boss (KHÔNG làm con của boss)
        if (fxCirclePrefab != null)
        {
            fxCircleInstance = Instantiate(
                fxCirclePrefab,
                transform.position,
                Quaternion.Euler(0, 0, 0) // Xoay ngang nằm trên mặt đất
            );
            fxCircleInstance.transform.localScale = new Vector3(1.3f, 1f, 1.3f);

            Renderer r = fxCircleInstance.GetComponentInChildren<Renderer>();
            if (r != null)
                fxMaterial = r.material;
        }
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Xoay mặt về phía player
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        if (lookPos.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), 5f * Time.deltaTime);

        if (!agent.pathPending && distance > agent.stoppingDistance)
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
        UpdateFXCircle();
    }

    void HandlePhases(float distance)
    {
        float hpPercent = (float)currentHP / maxHP * 100f;

        if (hpPercent > 75f)
        {
            if (distance <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                return;
            }
        }
        else if (hpPercent > 50f)
        {
            if (distance <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                return;
            }
            else if (distance > attackRange && distance <= throwRange && Time.time >= nextThrowTime)
            {
                Throw();
                return;
            }
        }
        else
        {
            if (distance <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                return;
            }
            else if (distance > attackRange && distance <= throwRange && Time.time >= nextThrowTime)
            {
                Throw();
                return;
            }
            else if (distance > throwRange && distance <= skillRange && Time.time >= nextSkillTime)
            {
                Skill();
                return;
            }
        }
    }

    void Attack()
    {
        anim.SetTrigger("attackTrigger");
        nextAttackTime = Time.time + attackCooldown;
        Debug.Log("🗡 Boss Attack (Melee)!");
    }

    void Throw()
    {
        anim.SetTrigger("throwTrigger");
        nextThrowTime = Time.time + throwCooldown;
        Debug.Log("🏹 Boss Throw (Ranged)!");
    }

    void Skill()
    {
        anim.SetTrigger("useSkill");
        nextSkillTime = Time.time + skillCooldown;
        Debug.Log("💥 Boss starts charging AOE skill!");
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

        yield return new WaitForSeconds(0.5f);
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
        Debug.Log("☠ Boss Dead!");
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

    // 💫 Hiệu ứng vòng dưới chân (xoay + phát sáng)
    void UpdateFXCircle()
    {
        if (fxCircleInstance == null) return;

        // Giữ vòng ở dưới chân boss
        fxCircleInstance.transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);

        // Xoay vòng quanh trục Y trong thế giới (không xoay theo boss)
        fxCircleInstance.transform.Rotate(Vector3.up, fxRotateSpeed * Time.deltaTime, Space.World);

        // Dao động phát sáng
        if (fxMaterial != null && fxMaterial.HasProperty("_EmissionColor"))
        {
            fxPulseTime += Time.deltaTime * fxPulseSpeed;
            float intensity = Mathf.Lerp(fxMinIntensity, fxMaxIntensity, (Mathf.Sin(fxPulseTime) + 1f) / 2f);
            Color baseColor = Color.cyan;
            fxMaterial.SetColor("_EmissionColor", baseColor * intensity);
        }
    }
}
