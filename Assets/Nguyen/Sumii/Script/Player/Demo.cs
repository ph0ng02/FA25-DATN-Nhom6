using UnityEngine;

public class Demo : MonoBehaviour
{
    [Header("Combat Settings")]
    public float comboResetTime = 1.0f;
    private int currentAttack = 0;
    private float lastAttackTime;
    private bool isAttacking = false;

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
    }

    void HandleAttack()
    {
        // Chuột trái tấn công
        if (Input.GetMouseButtonDown(0))
        {
            // Reset combo nếu lâu quá
            if (Time.time - lastAttackTime > comboResetTime)
                currentAttack = 0;

            currentAttack++;
            if (currentAttack > 4)
                currentAttack = 1;

            // Gửi trigger animation
            string triggerName = "Atk" + currentAttack;
            animator.SetTrigger(triggerName);

            // Bắt đầu tấn công
            isAttacking = true;
            lastAttackTime = Time.time;

            // Reset lại sau khi combo kết thúc
            CancelInvoke(nameof(ResetAttackState));
            Invoke(nameof(ResetAttackState), 0.8f); // thời gian animation đòn đánh
        }
    }

    void ResetAttackState()
    {
        isAttacking = false;
        animator.ResetTrigger("Atk1");
        animator.ResetTrigger("Atk2");
        animator.ResetTrigger("Atk3");
        animator.ResetTrigger("Atk4");
    }
}
