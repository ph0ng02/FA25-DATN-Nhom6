using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Demo2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float sprintSpeed = 6f;
    public float gravity = -9.81f;

    [Header("Combat Settings")]
    public float comboResetTime = 1.0f;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool canCancelNormal = false;

    private int currentAttack = 0;
    private float lastAttackTime;

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

    // ================== MOVEMENT ==================
    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;

        animator.SetFloat("InputHorizontal", h);
        animator.SetFloat("InputVertical", v);

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsSprinting", isSprinting);

        float movementMultiplier = (isAttacking) ? 0.4f : 1f;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            if (isSprinting && !isAttacking)
                speed = sprintSpeed;
            else if (Input.GetKey(KeyCode.LeftControl))
                speed = walkSpeed;
            else
                speed = runSpeed;

            controller.Move(moveDir * speed * movementMultiplier * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
        else speed = 0;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("Speed", speed * movementMultiplier);
    }

    // ================== NORMAL ATTACK ==================
    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastAttackTime > comboResetTime)
                currentAttack = 0;

            currentAttack++;
            if (currentAttack > 4) currentAttack = 1;

            string trigger = "Atk" + currentAttack;
            animator.ResetTrigger(trigger);
            animator.SetTrigger(trigger);

            isAttacking = true;
            canCancelNormal = false;
            lastAttackTime = Time.time;

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
