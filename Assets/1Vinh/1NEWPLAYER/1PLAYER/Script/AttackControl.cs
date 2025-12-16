using UnityEngine;
using System.Collections;

public class AttackControl : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;

    private int comboStep = 0;
    private float comboTimer = 0f;
    private float comboWindow = 0.6f;

    // ===============================
    // CIRCLE SLASH SKILL
    // ===============================
    [Header("Circle Slash Skill")]
    public float circleSlashCooldown = 5f;
    private float circleSlashTimer = 0f;
    public float circleSlashDamage = 40f;
    public float circleSlashRange = 3f;
    public LayerMask enemyLayer;

    // ===============================
    // JUMP SLASH SKILL
    // ===============================
    [Header("Jump Slash Skill")]
    public float jumpForce = 7f;
    public float jumpSlashDamage = 50f;
    public float jumpSlashRange = 3.5f;

    [Header("Jump Slash Cooldown")]
    public float jumpSlashCooldown = 6f;
    private float jumpSlashTimer = 0f;

    public GameObject jumpSlashVFX;
    public Transform vfxSpawnPoint;

    private bool isJumpSlashing = false;

    // ===============================
    // RISING SLASH SKILL (NEW)
    // ===============================
    [Header("Rising Slash Skill")]
    public float risingSlashDamage = 35f;
    public float risingSlashCooldown = 4f;
    private float risingSlashTimer = 0f;

    public GameObject risingSlashVFX;
    public Transform risingVFXSpawnPoint;

    private bool isRisingSlashing = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ===============================
        // COMBO TIMER
        // ===============================
        if (comboTimer > 0)
            comboTimer -= Time.deltaTime;
        else
            ResetCombo();

        // ===============================
        // JUMP SLASH INPUT
        // ===============================
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isJumpSlashing && jumpSlashTimer <= 0)
                JumpSlash();
        }

        // ===============================
        // RISING SLASH INPUT
        // ===============================
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!isRisingSlashing && risingSlashTimer <= 0)
                RisingSlash();
        }

        // ===============================
        // NORMAL ATTACK
        // ===============================
        if (Input.GetKeyDown(KeyCode.Mouse0)) DoCombo();
        if (Input.GetKeyDown(KeyCode.Y)) DoForwardAttack();
        if (Input.GetKeyDown(KeyCode.Mouse1)) DoHeavyAttack();

        // ===============================
        // CIRCLE SLASH
        // ===============================
        if (SkillManager.Instance != null && SkillManager.Instance.hasCircleSlash)
        {
            if (Input.GetKeyDown(KeyCode.F) && circleSlashTimer <= 0)
                CircleSlash();
        }

        // ===============================
        // COOLDOWN
        // ===============================
        if (circleSlashTimer > 0) circleSlashTimer -= Time.deltaTime;
        if (jumpSlashTimer > 0) jumpSlashTimer -= Time.deltaTime;
        if (risingSlashTimer > 0) risingSlashTimer -= Time.deltaTime;
    }

    // ===========================================================
    // CIRCLE SLASH
    // ===========================================================
    void CircleSlash()
    {
        anim.SetTrigger("CircleSlash");
        circleSlashTimer = circleSlashCooldown;
        Invoke(nameof(DoCircleSlashDamage), 0.25f);
        StartCoroutine(SpinEffect());
    }

    void DoCircleSlashDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, circleSlashRange, enemyLayer);
        foreach (Collider enemy in hits)
            if (enemy.TryGetComponent(out EnemyHealth eh))
                eh.TakeDamage((int)circleSlashDamage);
    }

    IEnumerator SpinEffect()
    {
        float t = 0;
        while (t < 0.35f)
        {
            transform.Rotate(0, 1200f * Time.deltaTime, 0);
            t += Time.deltaTime;
            yield return null;
        }
    }

    // ===========================================================
    // JUMP SLASH
    // ===========================================================
    void JumpSlash()
    {
        isJumpSlashing = true;
        jumpSlashTimer = jumpSlashCooldown;

        ResetCombo();
        anim.SetTrigger("JumpSlash");

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void OnJumpSlashHit()
    {
        if (jumpSlashVFX && vfxSpawnPoint)
            Instantiate(jumpSlashVFX, vfxSpawnPoint.position, Quaternion.LookRotation(transform.forward));

        Collider[] hits = Physics.OverlapSphere(transform.position, jumpSlashRange, enemyLayer);
        foreach (Collider enemy in hits)
            if (enemy.TryGetComponent(out EnemyHealth eh))
                eh.TakeDamage((int)jumpSlashDamage);
    }

    public void EndJumpSlash()
    {
        isJumpSlashing = false;
    }

    // ===========================================================
    // RISING SLASH (NEW)
    // ===========================================================
    void RisingSlash()
    {
        isRisingSlashing = true;
        risingSlashTimer = risingSlashCooldown;

        ResetCombo();
        anim.SetTrigger("RisingSlash");
    }

    // ⚠ Animation Event
    public void OnRisingSlashHit()
    {
        if (risingSlashVFX && risingVFXSpawnPoint)
        {
            Instantiate(
                risingSlashVFX,
                risingVFXSpawnPoint.position,
                Quaternion.LookRotation(transform.forward)
            );
        }
    }

    public void EndRisingSlash()
    {
        isRisingSlashing = false;
    }

    // ===========================================================
    // COMBO
    // ===========================================================
    void DoCombo()
    {
        comboStep = comboStep % 5 + 1;
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
