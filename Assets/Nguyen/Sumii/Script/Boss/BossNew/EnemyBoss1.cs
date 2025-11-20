using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyBoss11 : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;
    public NavMeshAgent agent;
    public Transform player;

    [Header("Beam Skill")]
    public GameObject beamPrefab;
    public float beamRadius = 2.5f;
    public float beamDuration = 3f;
    public float beamRotateSpeed = 150f;

    [Header("Water Impact Skill")]
    public GameObject waterImpactPrefab;
    public float waterSpawnHeight = 10f;       // độ cao để water impact rơi xuống
    public float waterDamageRadius = 1f;       // vùng trúng
    public float waterInterval = 15f;          // mỗi 15s dùng 1 lần
    private float nextWaterTime = 0f;

    [Header("Stats")]
    public int maxHP = 1000;
    private int currentHP;

    private bool isAttacking = false;
    public float attackRange = 3f;

    private float skillCooldown = 0f;

    void Start()
    {
        currentHP = maxHP;
        nextWaterTime = Time.time + waterInterval;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Boss di chuyển tới player
        if (dist > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("Run", true);
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("Run", false);

            if (!isAttacking && Time.time >= skillCooldown)
                TryAttack();
        }

        // Tự sử dụng Skill Water Impact
        if (Time.time >= nextWaterTime)
        {
            StartCoroutine(DoWaterImpact());
            nextWaterTime = Time.time + waterInterval;
        }
    }

    // -------------------- CHỌN SKILL --------------------
    void TryAttack()
    {
        int r = Random.Range(0, 4);

        switch (r)
        {
            case 0: StartCoroutine(DoAttack1()); break;
            case 1: StartCoroutine(DoAttack2()); break;
            case 2: StartCoroutine(DoAttack3()); break;
            case 3: StartCoroutine(DoBeamSkill()); break;
        }
    }

    // =============== NORMAL ATTACKS ===============
    IEnumerator DoAttack1()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.2f);
        EndAttack();
    }

    IEnumerator DoAttack2()
    {
        isAttacking = true;
        anim.SetTrigger("DoAttack2");
        yield return new WaitForSeconds(1.3f);
        EndAttack();
    }

    IEnumerator DoAttack3()
    {
        isAttacking = true;
        anim.SetTrigger("DoAttack3");
        yield return new WaitForSeconds(1.6f);
        EndAttack();
    }

    // =============== BEAM SKILL (XOAY 3 TIA) ===============
    IEnumerator DoBeamSkill()
    {
        isAttacking = true;
        anim.SetTrigger("Cast");
        yield return new WaitForSeconds(0.8f);

        Transform[] beams = new Transform[3];

        for (int i = 0; i < 3; i++)
        {
            float angle = i * 120f;
            Vector3 pos = transform.position +
                          Quaternion.Euler(0, angle, 0) * Vector3.forward * beamRadius;

            GameObject b = Instantiate(beamPrefab, pos, Quaternion.identity);
            beams[i] = b.transform;

            Vector3 dir = (b.transform.position - transform.position).normalized;
            b.transform.rotation = Quaternion.LookRotation(dir);
        }

        float timer = 0;
        while (timer < beamDuration)
        {
            float rotateStep = beamRotateSpeed * Time.deltaTime;

            foreach (Transform t in beams)
                if (t != null)
                    t.RotateAround(transform.position, Vector3.up, rotateStep);

            timer += Time.deltaTime;
            yield return null;
        }

        foreach (Transform t in beams)
            if (t != null)
                Destroy(t.gameObject);

        EndAttack();
    }

    // =======================================================
    //               WATER IMPACT FALLING SKILL
    // =======================================================
    IEnumerator DoWaterImpact()
    {
        // animation (nếu boss có skill cast animation)
        anim.SetTrigger("Cast");

        // spawn từ 10–15 giọt
        int dropAmount = Random.Range(10, 16);

        for (int i = 0; i < dropAmount; i++)
        {
            // vị trí player lúc tạo giọt
            Vector3 spawnPos = player.position + new Vector3(
                Random.Range(-2f, 2f),
                waterSpawnHeight,
                Random.Range(-2f, 2f)
            );

            GameObject drop = Instantiate(waterImpactPrefab, spawnPos, Quaternion.identity);

            // kiểm tra va chạm khi rơi xong
            StartCoroutine(CheckWaterHit(drop));

            yield return new WaitForSeconds(0.1f); // rơi liên tục
        }
    }

    IEnumerator CheckWaterHit(GameObject drop)
    {
        // đợi giọt rơi xuống đất
        yield return new WaitForSeconds(0.6f);

        if (Vector3.Distance(drop.transform.position, player.position) < waterDamageRadius)
        {
            // Player trúng → mất 1 HP
            player.GetComponent<PlayerHealth>().TakeDamage(1);
        }

        Destroy(drop);
    }

    // ======================= END ATTACK =======================
    void EndAttack()
    {
        isAttacking = false;
        skillCooldown = Time.time + 1.2f;
    }

    // ======================= DAMAGE =======================
    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            currentHP = 0;
            // TODO: animation die
        }
    }
}
