using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Thành phần Animator")]
    public Animator animator;

    void Awake()
    {
        // Tự động tìm animator trong object con nếu chưa gán
        if (animator == null)
            animator = GetComponentInChildren<Animator>(true);
    }

    // Hàm được PlayerController gọi mỗi frame
    public void UpdateAnimator(float speed, bool isSprinting, bool isGrounded)
    {
        if (animator == null) return;

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsSprinting", isSprinting);
        animator.SetBool("IsGrounded", isGrounded);
    }
}
