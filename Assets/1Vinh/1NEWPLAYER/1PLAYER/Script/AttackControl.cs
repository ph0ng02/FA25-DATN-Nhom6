using UnityEngine;

public class AttackControl : MonoBehaviour
{
    private Animator anim;
    private int comboStep = 0;
    private float comboTimer = 0f;
    private float comboWindow = 0.6f;

    [Header("Circle Slash Skill")]
    public float circleSlashCooldown = 5f;
    private float circleSlashTimer = 0f;
    public float circleSlashDamage = 40f; // DAMAGE LÀ FLOAT
    public float circleSlashRange = 3f;
    public LayerMask enemyLayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Combo timer giảm dần
        if (comboTimer > 0)
            comboTimer -= Time.deltaTime;
        else
            ResetCombo();

        // Combo chuột trái
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DoCombo();
        }

        // Forward Attack (Y key)
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DoForwardAttack();
        }

        // Heavy Attack (Right Mouse)
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            DoHeavyAttack();
        }

        // Circle Slash Skill (F)
        if (SkillManager.Instance.circleSlashUnlocked &&
            Input.GetKeyDown(KeyCode.F) &&
            circleSlashTimer <= 0)
        {
            CircleSlash();
        }

        // Giảm cooldown skill
        if (circleSlashTimer > 0)
            circleSlashTimer -= Time.deltaTime;
    }

    // ===========================================================
    //  CIRCLE SLASH SKILL
    // ===========================================================

    void CircleSlash()
    {
        Debug.Log("Dùng Circle Slash!");

        anim.SetTrigger("CircleSlash");

        circleSlashTimer = circleSlashCooldown; // Reset cooldown

        // Gây damage sau 0.25 giây cho khớp animation
        Invoke(nameof(DoCircleSlashDamage), 0.25f);

        // Hiệu ứng xoay
        StartCoroutine(SpinEffect());
    }

    System.Collections.IEnumerator SpinEffect()
    {
        float duration = 0.35f;
        float rotateSpeed = 1200f;
        float t = 0;

        while (t < duration)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
            t += Time.deltaTime;
            yield return null;
        }
    }

    void DoCircleSlashDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, circleSlashRange, enemyLayer);

        foreach (Collider enemy in hits)
        {
            if (enemy.TryGetComponent(out EnemyHealth eh))
            {
                // damage là float → EnemyHealth phải nhận float
                eh.TakeDamage((int)circleSlashDamage);
            }
        }

        Debug.Log($"CircleSlash đánh trúng {hits.Length} kẻ địch!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleSlashRange);
    }

    // ===========================================================
    //  COMBO + ATTACKS
    // ===========================================================

    void DoCombo()
    {
        comboStep++;

        if (comboStep > 5)
            comboStep = 1;

        anim.SetInteger("Combo", comboStep);
        anim.SetBool("Attack", true);

        comboTimer = comboWindow;
    }

    void DoForwardAttack()
    {
        ResetCombo();
        anim.SetTrigger("Forward");
    }

    void DoHeavyAttack()
    {
        ResetCombo();
        anim.SetTrigger("Heavy");
    }

    public void EndAttack()
    {
        anim.SetBool("Attack", false);
    }

    void ResetCombo()
    {
        comboStep = 0;
        anim.SetInteger("Combo", 0);
        anim.SetBool("Attack", false);
    }
}
