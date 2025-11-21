using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Demo22Keyboard : MonoBehaviour
{
    [Header("Combat Settings")]
    public float comboResetTime = 1.0f;
    private int currentAttack = 0;
    private float lastAttackTime;
    private bool isAttacking = false;

    [Header("Special Attack Settings (Q)")]
    public float specialComboResetTime = 2.0f;
    private int currentSpecialAttack = 0;
    private float lastSpecialAttackTime;
    private bool isUsingSpecial = false;
    private bool canCancelSpecial = false;
    private bool canCancelNormal = false;
    private float qStartTime;
    public float qAutoCancelTime = 1.0f;

    [Header("References")]
    public Animator animator;

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleAttack();
        HandleSpecialAttack();
        CheckAutoCancelQ();
    }

    // ================== NORMAL ATTACK ==================
    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isUsingSpecial && canCancelSpecial)
                StopSpecialImmediately();

            if (Time.time - lastAttackTime > comboResetTime)
                currentAttack = 0;

            currentAttack++;
            if (currentAttack > 4)
                currentAttack = 1;

            string triggerName = "Atk" + currentAttack;
            animator.ResetTrigger(triggerName);
            animator.SetTrigger(triggerName);

            isAttacking = true;
            canCancelNormal = false;
            animator.SetBool("IsAttacking", true);

            lastAttackTime = Time.time;
            CancelInvoke(nameof(EnableCancelNormal));
            CancelInvoke(nameof(ResetAttackState));

            Invoke(nameof(EnableCancelNormal), 0.1f);
            Invoke(nameof(ResetAttackState), 0.9f);
        }
    }

    void EnableCancelNormal() => canCancelNormal = true;

    void ResetAttackState()
    {
        isAttacking = false;
        canCancelNormal = false;
        animator.SetBool("IsAttacking", false);
        for (int i = 1; i <= 4; i++)
            animator.ResetTrigger("Atk" + i);
    }

    // ================== SPECIAL ATTACK (Q) ==================
    void HandleSpecialAttack()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isAttacking && canCancelNormal)
                StopNormalAttackImmediately();
            else if (isUsingSpecial && canCancelSpecial)
            {
                StopSpecialImmediately();
                return;
            }

            if (Time.time - lastSpecialAttackTime > specialComboResetTime)
                currentSpecialAttack = 4;

            currentSpecialAttack++;
            if (currentSpecialAttack > 8)
                currentSpecialAttack = 5;

            string triggerName = "Atk" + currentSpecialAttack;
            animator.ResetTrigger(triggerName);
            animator.SetTrigger(triggerName);

            isUsingSpecial = true;
            canCancelSpecial = false;
            animator.SetBool("IsUsingSpecial", true);
            lastSpecialAttackTime = Time.time;
            qStartTime = Time.time;

            CancelInvoke(nameof(EnableCancelSpecial));
            CancelInvoke(nameof(ResetSpecialAttackState));

            Invoke(nameof(EnableCancelSpecial), 0.1f);
            Invoke(nameof(ResetSpecialAttackState), 1.0f);
        }
    }

    void EnableCancelSpecial() => canCancelSpecial = true;

    void ResetSpecialAttackState()
    {
        isUsingSpecial = false;
        canCancelSpecial = false;
        animator.SetBool("IsUsingSpecial", false);
        for (int i = 5; i <= 8; i++)
            animator.ResetTrigger("Atk" + i);
    }

    void CheckAutoCancelQ()
    {
        if (isUsingSpecial && (Time.time - qStartTime) >= qAutoCancelTime && !isAttacking)
            StopSpecialImmediately();
    }

    void StopNormalAttackImmediately()
    {
        CancelInvoke(nameof(ResetAttackState));
        isAttacking = false;
        canCancelNormal = false;
        animator.SetBool("IsAttacking", false);
        for (int i = 1; i <= 4; i++)
            animator.ResetTrigger("Atk" + i);
    }

    void StopSpecialImmediately()
    {
        CancelInvoke(nameof(ResetSpecialAttackState));
        isUsingSpecial = false;
        canCancelSpecial = false;
        animator.SetBool("IsUsingSpecial", false);
        for (int i = 5; i <= 8; i++)
            animator.ResetTrigger("Atk" + i);
    }
}
