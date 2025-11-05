using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Demo : MonoBehaviour
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
    }

    void HandleMovement()
    {
        // Kiểm tra tiếp đất
        isGrounded = controller.isGrounded;
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Input từ bàn phím
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        animator.SetFloat("InputHorizontal", horizontal);
        animator.SetFloat("InputVertical", vertical);

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsSprinting", isSprinting);

        // Nếu đang tấn công, cho phép di chuyển chậm hơn (hoặc vẫn di chuyển nhẹ)
        float movementMultiplier = isAttacking ? 0.4f : 1f;

        if (direction.magnitude >= 0.1f)
        {
            // Xoay theo hướng camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.Normalize();

            // Tốc độ
            if (isSprinting && !isAttacking)
                speed = sprintSpeed;
            else if (Input.GetKey(KeyCode.LeftControl))
                speed = walkSpeed;
            else
                speed = runSpeed;

            // Di chuyển nhân vật
            controller.Move(moveDir * speed * movementMultiplier * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
        else
        {
            speed = 0f;
        }

        // Trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Đồng bộ Animator
        float inputMagnitude = direction.magnitude;
        animator.SetFloat("InputMagnitude", inputMagnitude, 0.1f, Time.deltaTime);
        animator.SetFloat("Speed", speed * movementMultiplier);
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

            // Gọi reset sau khi combo kết thúc
            CancelInvoke(nameof(ResetAttackState));
            Invoke(nameof(ResetAttackState), 0.8f); // Thời gian bằng độ dài animation
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
