using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Attack Settings")]
    public float comboResetTime = 1.0f;  // thời gian reset combo nếu không đánh tiếp
    private int currentAttack = 0;        // đánh lần thứ mấy
    private float lastAttackTime;         // thời điểm đánh gần nhất

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
        // Nếu bấm chuột trái (hoặc phím tấn công)
        if (Input.GetMouseButtonDown(0))
        {
            // Reset combo nếu thời gian giữa 2 đòn quá lâu
            if (Time.time - lastAttackTime > comboResetTime)
                currentAttack = 0;

            // Tăng combo
            currentAttack++;
            lastAttackTime = Time.time;

            // Giới hạn combo trong 4 đòn
            if (currentAttack > 4)
                currentAttack = 1;

            // Gửi trigger tới Animator
            string triggerName = "Atk" + currentAttack;
            animator.SetTrigger(triggerName);

            Debug.Log("Attack: " + triggerName);
        }
    }
}
