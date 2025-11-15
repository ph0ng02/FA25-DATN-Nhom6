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
    public float beamRange = 12f; // phạm vi beam

    [Header("Cooldowns")]
    public float attackCooldown = 2f;
    public float throwCooldown = 4f;
    public float skillCooldown = 8f;
    public float beamCooldown = 10f;

    private float nextAttackTime;
    private float nextThrowTime;
    private float nextSkillTime;

    [Header("AOE Skill")]
    public GameObject chargeEffectPrefab;
    public GameObject skillEffectPrefab;
    public Transform chargePoint;
    public float skillChargeTime = 2f;
    public int skillDamage = 100;
    public float skillAOERadius = 5f;

    private GameObject currentChargeEffect;

    [Header("Beam Skill")]
    public GameObject beamPrefab;
    public Transform beamFirePoint;
    public float beamDuration = 3f;
    public int beamDamage = 15;
    public float beamHitRadius = 1.5f;

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

        // Hiệu ứng vòng dưới chân
        if (fxCirclePrefab != null)
        {
            fxCircleInstance = Instantiate(fxCirclePrefab, transform.position, Quaternion.identity);
            fxCircleInstance.transform.localScale = new Vector3(1.3f, 1f, 1.3f);
            Renderer r = fxCircleInstance.GetComponentInChildren<Renderer>();
            if (r != null) fxMaterial = r.material;
        }

        // Bắt đầu coroutine tự bắn beam mỗi 10s
        StartCoroutine(AutoBeamRoutine());
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
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
        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
        }
        else if (distance <= throwRange && Time.time >= nextThrowTime)
        {
            Throw();
        }
        else if (distance <= skillRange && Time.time >= nextSkillTime)
        {
            SkillAOE();
        }
        // Beam bây giờ được gọi tự động trong AutoBeamRoutine()
    }

    void Attack()
    {
        anim.SetTrigger("attackTrigger");
        nextAttackTime = Time.time + attackCooldown;
    }

    void Throw()
    {
        anim.SetTrigger("throwTrigger");
        nextThrowTime = Time.time + throwCooldown;
    }

    void SkillAOE()
    {
        anim.SetTrigger("useSkill");
        nextSkillTime = Time.time + skillCooldown;
        agent.isStopped = true;

        if (chargeEffectPrefab && chargePoint)
            currentChargeEffect = Instantiate(chargeEffectPrefab, chargePoint.position, chargePoint.rotation, chargePoint);

        StartCoroutine(CastAOEAfterCharge());
    }

    private IEnumerator CastAOEAfterCharge()
    {
        yield return new WaitForSeconds(skillChargeTime);

        if (currentChargeEffect != null)
            Destroy(currentChargeEffect);

        if (skillEffectPrefab && chargePoint)
            Instantiate(skillEffectPrefab, chargePoint.position, Quaternion.identity);

        if (chargePoint)
        {
            Collider[] hits = Physics.OverlapSphere(chargePoint.position, skillAOERadius);
            foreach (var hit in hits)
                if (hit.CompareTag("Player"))
                    hit.GetComponent<PlayerHealth>()?.TakeDamage(skillDamage);
        }

        agent.isStopped = false;
    }

    IEnumerator UseBeamSkill()
    {
        anim.SetTrigger("useBeam");
        agent.isStopped = true;

        // Quay mặt về player trước khi bắn
        if (player != null)
        {
            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        yield return new WaitForSeconds(1f); // niệm chiêu 1 giây

        if (beamPrefab && beamFirePoint)
        {
            GameObject beam = Instantiate(beamPrefab, beamFirePoint.position, beamFirePoint.rotation);
            StartCoroutine(TrackBeamToPlayer(beam));
        }

        yield return new WaitForSeconds(beamDuration);
        agent.isStopped = false;
    }

    IEnumerator TrackBeamToPlayer(GameObject beam)
    {
        float elapsed = 0f;
        float moveSpeed = 12f; // tốc độ beam bay

        while (elapsed < beamDuration && player != null && beam != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > beamRange) // nếu player ra khỏi phạm vi
                break;

            // beam tự hướng theo player
            Vector3 targetPos = player.position + Vector3.up * 1.2f;
            Vector3 dir = (targetPos - beam.transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(dir);
            beam.transform.rotation = Quaternion.Lerp(beam.transform.rotation, targetRot, Time.deltaTime * 10f);
            beam.transform.position += dir * moveSpeed * Time.deltaTime;

            // Kiểm tra va chạm player
            RaycastHit hit;
            if (Physics.SphereCast(beam.transform.position, beamHitRadius, dir, out hit, 0.5f))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(beamDamage);
                    Destroy(beam);
                    yield break;
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (beam != null) Destroy(beam);
    }

    IEnumerator AutoBeamRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(beamCooldown); // 10s 1 lần

            if (player == null) continue;
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= beamRange) // chỉ bắn khi player trong phạm vi
            {
                StartCoroutine(UseBeamSkill());
            }
        }
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
        Destroy(gameObject, 5f);
    }

    void UpdateFXCircle()
    {
        if (fxCircleInstance == null) return;
        fxCircleInstance.transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        fxCircleInstance.transform.Rotate(Vector3.up, fxRotateSpeed * Time.deltaTime, Space.World);

        if (fxMaterial != null && fxMaterial.HasProperty("_EmissionColor"))
        {
            fxPulseTime += Time.deltaTime * fxPulseSpeed;
            float intensity = Mathf.Lerp(fxMinIntensity, fxMaxIntensity, (Mathf.Sin(fxPulseTime) + 1f) / 2f);
            fxMaterial.SetColor("_EmissionColor", Color.cyan * intensity);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (chargePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(chargePoint.position, skillAOERadius);
        }

        // Vẽ phạm vi beam
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, beamRange);
    }
}
