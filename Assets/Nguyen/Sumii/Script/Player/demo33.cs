using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Demo33 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float sprintSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

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
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float speed;
    private bool isSprinting;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
        HandleSpecialAttack();
        CheckAutoCancelQ();
    }

    // ================== MOVEMENT ==================
    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        animator.SetFloat("InputHorizontal", horizontal);
        animator.SetFloat("InputVertical", vertical);

        // Nhấn Shift để chạy nhanh
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsSprinting", isSprinting);

        // Giảm tốc nếu đang tấn công
        float movementMultiplier = (isAttacking || isUsingSpecial) ? 0.4f : 1f;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Tốc độ dựa trên trạng thái
            if (isSprinting && !isAttacking && !isUsingSpecial)
                speed = sprintSpeed;
            else if (Input.GetKey(KeyCode.LeftControl))
                speed = walkSpeed;
            else
                speed = runSpeed;

            controller.Move(moveDir.normalized * speed * movementMultiplier * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
        else
        {
            speed = 0f;
        }

        // Áp lực trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Gửi dữ liệu vào Animator
        float inputMag = new Vector2(horizontal, vertical).magnitude;
        animator.SetFloat("InputMagnitude", inputMag, 0.1f, Time.deltaTime);
        animator.SetFloat("Speed", speed * movementMultiplier);
    }

    // ================== JUMP ==================
    void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
            animator.SetBool("IsGrounded", false);
        }
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

    // ================== FORCE STOP HELPERS ==================
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
