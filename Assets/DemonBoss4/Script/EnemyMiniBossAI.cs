using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMiniBossAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Animator animator;
    private Transform player;

    [Header("Skill VFX")]
    public GameObject skill1VFX;
    public GameObject skill2VFX;

    [Header("Skill Spawn Points")]
    public Transform skill2Point;

    [Header("Ranges")]
    public float detectRange = 12f;
    public float attackRange = 2.5f;

    [Header("Cooldown Settings")]
    public float skill1Cooldown = 5f;
    public float skill2Cooldown = 8f;

    private float nextSkill1Time = 0f;
    private float nextSkill2Time = 0f;

    private bool isUsingSkill2 = false;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Nếu đang dùng Skill 2 → đứng yên hoàn toàn
        if (isUsingSkill2)
        {
            agent.isStopped = true;
            animator.SetBool("Walk", false);
            return;
        }

        // Ngoài phạm vi → Idle
        if (distance > detectRange)
        {
            agent.isStopped = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
            return;
        }
        else animator.SetBool("Idle", false);

        // Ưu tiên Skill 2
        if (Time.time >= nextSkill2Time)
        {
            StartCoroutine(DoSkill2());
            nextSkill2Time = Time.time + skill2Cooldown;
            return;
        }

        // Kế đến Skill 1
        if (Time.time >= nextSkill1Time)
        {
            StartCoroutine(DoSkill1());
            nextSkill1Time = Time.time + skill1Cooldown;
            return;
        }

        // Đánh thường
        if (distance <= attackRange)
        {
            ResetMovement();
            StartCoroutine(DoAttack());
            return;
        }

        // Chase Player
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    // ============================
    //        SKILL 1: Explosion
    // ============================
    public void SpawnSkill1VFX()
    {
        StartCoroutine(Skill1Explosion());
    }

    private IEnumerator Skill1Explosion()
    {
        Vector3 pos = player.position;
        Quaternion rot = Quaternion.LookRotation(player.position - transform.position);

        GameObject fx = Instantiate(skill1VFX, pos, rot);

        // Chờ đúng thời điểm nổ
        yield return new WaitForSeconds(1.2f);

        float damage = 25f;
        float radius = 3f;

        // Lấy tất cả collider trong radius
        Collider[] hits = Physics.OverlapSphere(pos, radius);
        foreach (var hit in hits)
        {
            // Chỉ gây damage nếu object là player
            if (hit.CompareTag("Player"))
            {
                IDamageable dmg = hit.GetComponent<IDamageable>();
                if (dmg != null)
                    dmg.TakeDamage(damage);
            }
        }

        Destroy(fx, 2f);
    }

    // ============================
    //         SKILL 2
    // ============================
    public void SpawnSkill2VFX()
    {
        StartCoroutine(Skill2Damage());
    }

    private IEnumerator Skill2Damage()
    {
        GameObject fx = Instantiate(skill2VFX, skill2Point.position, skill2Point.rotation);

        yield return new WaitForSeconds(0.8f);

        float damage = 35f;
        float radius = 3.5f;

        Collider[] hits = Physics.OverlapSphere(skill2Point.position, radius);
        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponent<IDamageable>();
            if (dmg != null)
                dmg.TakeDamage(damage);
        }

        Destroy(fx, 2f);
    }

    // ============================
    //      NORMAL ATTACK
    // ============================
    private IEnumerator DoAttack()
    {
        ResetMovement();
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(2f);
    }

    // ============================
    //        SKILL 1
    // ============================
    private IEnumerator DoSkill1()
    {
        ResetMovement();
        animator.SetTrigger("Skill1");
        yield return new WaitForSeconds(2f);
    }

    // ============================
    //        SKILL 2 (đứng yên)
    // ============================
    private IEnumerator DoSkill2()
    {
        isUsingSkill2 = true;
        ResetMovement();
        animator.SetTrigger("Skill2");

        yield return new WaitForSeconds(4f);

        isUsingSkill2 = false;
    }

    private void ResetMovement()
    {
        agent.isStopped = true;
        animator.SetBool("Walk", false);
    }
}
