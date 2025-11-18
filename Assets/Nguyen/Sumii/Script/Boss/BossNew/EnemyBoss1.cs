using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EmenyBoss11 : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent agent;
    public Transform player;

    public GameObject beamPrefab;    // Prefab tia Beam
    public float beamRadius = 2.5f;  // khoảng cách từ boss -> tia
    public float beamDuration = 3f;  // thời gian tồn tại
    public float beamRotateSpeed = 150f; // tốc độ xoay

    public int maxHP = 1000;
    private int currentHP;

    private bool isAttacking = false;
    public float attackRange = 3f;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("Run", true);
            return;
        }

        agent.isStopped = true;
        anim.SetBool("Run", false);

        if (!isAttacking)
            TryAttack();
    }

    void TryAttack()
    {
        int r = Random.Range(0, 4); // 0-3 (4 skill)

        if (r == 0)
            StartCoroutine(DoAttack1());
        else if (r == 1)
            StartCoroutine(DoAttack2());
        else if (r == 2)
            StartCoroutine(DoAttack3());
        else
            StartCoroutine(DoBeamSkill()); // SKILL BEAM XOAY QUANH
    }

    IEnumerator DoAttack1()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1.2f);
        isAttacking = false;
    }

    IEnumerator DoAttack2()
    {
        isAttacking = true;
        anim.SetTrigger("DoAttack2");
        yield return new WaitForSeconds(1.3f);
        isAttacking = false;
    }

    IEnumerator DoAttack3()
    {
        isAttacking = true;
        anim.SetTrigger("DoAttack3");
        yield return new WaitForSeconds(1.6f);
        isAttacking = false;
    }

    // ================================================
    //                SKILL BEAM 3 TIA XOAY
    // ================================================
    IEnumerator DoBeamSkill()
    {
        isAttacking = true;
        anim.SetTrigger("Cast"); // animation thi triển skill

        yield return new WaitForSeconds(0.8f); // đợi anim bắt đầu

        // Tạo 3 tia
        Transform[] beams = new Transform[3];

        for (int i = 0; i < 3; i++)
        {
            float angle = i * 120f; // 3 tia cách nhau 120 độ
            Vector3 pos = transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * beamRadius;

            GameObject b = Instantiate(beamPrefab, pos, Quaternion.identity);
            beams[i] = b.transform;

            // Xoay tia hướng ra ngoài
            beams[i].LookAt(transform.position);
            beams[i].Rotate(0, 180f, 0);  // quay ngược lại đúng hướng
        }

        float timer = 0;

        while (timer < beamDuration)
        {
            float rotate = beamRotateSpeed * Time.deltaTime;

            foreach (Transform t in beams)
            {
                if (t != null)
                {
                    // Xoay quanh boss
                    t.RotateAround(transform.position, Vector3.up, rotate);
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Xóa beam khi xong
        foreach (Transform t in beams)
        {
            if (t != null) Destroy(t.gameObject);
        }

        isAttacking = false;
    }

    // =================================================

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP < 0) currentHP = 0;
    }
}
