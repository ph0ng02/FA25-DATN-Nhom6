using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Boss1 : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent agent;
    public Transform player;

    public int maxHP = 1000;
    private int currentHP;

    private bool isAttacking = false;
    public float attackRange = 3f;   // tầm đánh gần

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Nếu player ở xa → chạy tới
        if (dist > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("Run", true);
            return;
        }

        // Player ở gần → tấn công
        agent.isStopped = true;
        anim.SetBool("Run", false);

        if (!isAttacking)
            TryAttack();
    }

    void TryAttack()
    {
        float hpPercent = (float)currentHP / maxHP;

        // BOSS TRÊN 50% MÁU → Attack1 và Attack2
        if (hpPercent > 0.5f)
        {
            int r = Random.Range(0, 2); // 0 hoặc 1

            if (r == 0)
                StartCoroutine(DoAttack1());
            else
                StartCoroutine(DoAttack2());
        }
        else
        {
            StartCoroutine(DoAttack3());
        }
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
        yield return new WaitForSeconds(1.2f);
        isAttacking = false;
    }

    IEnumerator DoAttack3()
    {
        isAttacking = true;
        anim.SetTrigger("DoAttack3");
        yield return new WaitForSeconds(1.6f);
        isAttacking = false;
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP < 0) currentHP = 0;
    }
}
