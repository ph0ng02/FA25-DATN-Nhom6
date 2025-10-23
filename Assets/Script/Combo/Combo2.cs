using UnityEngine;
using System.Collections;

public class Combo2 : MonoBehaviour
{
    private Animator animator;
    private int comboStep = 0;
    private float comboTimer = 0f;
    public float comboDelay = 1.0f; // Thời gian giữa 2 cú Q
    private bool isAttacking = false;

    void Start()
    {
        // ✅ Tự động lấy Animator (khỏi cần gán bằng tay)
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Ấn Q để đánh combo
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking)
        {
            // Nếu để lâu không combo → reset
            if (Time.time - comboTimer > comboDelay)
                comboStep = 0;

            comboStep++;
            comboTimer = Time.time;

            if (comboStep > 4)
                comboStep = 1; // reset vòng combo

            // Tên trigger trong Animator
            string triggerName = "Atk" + (comboStep + 4); // Atk5 → Atk8
            animator.SetTrigger(triggerName);

            // ✅ Khóa nút Q cho tới khi animation xong
            isAttacking = true;

            // Lấy thời lượng animation hiện tại (nếu có)
            float attackDuration = GetCurrentAnimationLength(triggerName);
            if (attackDuration <= 0f) attackDuration = 0.7f; // fallback

            StartCoroutine(UnlockAttackAfterDelay(attackDuration * 0.9f)); // cho phép combo sớm hơn chút
        }

        // Reset combo nếu chờ quá lâu
        if (comboStep > 0 && Time.time - comboTimer > comboDelay)
        {
            comboStep = 0;
        }
    }

    IEnumerator UnlockAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    // ✅ Hàm lấy thời lượng animation hiện tại
    float GetCurrentAnimationLength(string animName)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return 0f;

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animName)
                return clip.length;
        }
        return 0f;
    }
}
