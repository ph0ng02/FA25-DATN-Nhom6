using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Demo22 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float sprintSpeed = 6f;
    public float gravity = -9.81f;

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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
        HandleSpecialAttack();
        CheckAutoCancelQ();

        // ✅ Giúp animator trở lại di chuyển sau khi đánh xong
        if (!isAttacking && !isUsingSpecial)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            animator.SetFloat("InputMagnitude", new Vector2(horizontal, vertical).magnitude);
        }
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

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsSprinting", isSprinting);

        // ✅ Nếu đang đánh hay dùng chiêu thì giảm tốc
        float movementMultiplier = (isAttacking || isUsingSpecial) ? 0.4f : 1f;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.Normalize();

            if (isSprinting && !isAttacking && !isUsingSpecial)
                speed = sprintSpeed;
            else if (Input.GetKey(KeyCode.LeftControl))
                speed = walkSpeed;
            else
                speed = runSpeed;

            controller.Move(moveDir * speed * movementMultiplier * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
        else
        {
            speed = 0f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("Speed", speed * movementMultiplier);
    }

    // ================== NORMAL ATTACK ==================
    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ✅ Nếu đang dùng Q → hủy Q
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
            // ✅ Nếu đang đánh thường → hủy đánh thường và ra chiêu Q
            if (isAttacking && canCancelNormal)
                StopNormalAttackImmediately();

            // ✅ Nếu đang Q và nhấn lại Q → hủy Q
            else if (isUsingSpecial && canCancelSpecial)
            {
                StopSpecialImmediately();
                return;
            }

            // ✅ Reset combo Q nếu quá lâu
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

    // ✅ Tự hủy Q nếu để quá lâu không combo
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
