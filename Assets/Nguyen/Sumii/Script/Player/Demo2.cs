using UnityEngine;

public class Demo2 : MonoBehaviour
{
    [Header("Combat Settings")]
    public float comboResetTime = 1.0f;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool canCancelNormal = false;

    private int currentAttack = 0;
    private float lastAttackTime;

    [Header("References")]
    public Animator animator;

    [Header("Player Settings")]
    [Tooltip("Đặt tên input để phân biệt player, ví dụ: Attack1 hoặc Attack2")]
    public string attackButton = "Attack1"; // Mặc định cho Player 1

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleAttack();
    }

    // ================== NORMAL ATTACK ==================
    void HandleAttack()
    {
        // Nhấn nút tấn công từ tay cầm
        if (Input.GetButtonDown(attackButton))
        {
            // Reset combo nếu lâu quá
            if (Time.time - lastAttackTime > comboResetTime)
                currentAttack = 0;

            currentAttack++;
            if (currentAttack > 4)
                currentAttack = 1;

            // Gửi trigger animation
            string trigger = "Atk" + currentAttack;
            animator.ResetTrigger(trigger);
            animator.SetTrigger(trigger);

            // Đánh dấu trạng thái
            isAttacking = true;
            canCancelNormal = false;
            lastAttackTime = Time.time;

            // Thời gian cancel & reset
            CancelInvoke(nameof(EnableCancelNormal));
            CancelInvoke(nameof(ResetAttackState));
            Invoke(nameof(EnableCancelNormal), 0.1f);
            Invoke(nameof(ResetAttackState), 0.9f);
        }
    }

    void EnableCancelNormal() => canCancelNormal = true;

    public void ResetAttackState()
    {
        isAttacking = false;
        canCancelNormal = false;

        for (int i = 1; i <= 4; i++)
            animator.ResetTrigger("Atk" + i);

        animator.CrossFade("Free Locomotion", 0.1f);
    }
}
