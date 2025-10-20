using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController1 : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    public float walkSpeed = 2f;       // tốc độ đi bộ
    public float runSpeed = 5f;        // tốc độ chạy
    public float sprintSpeed = 8f;     // tốc độ chạy nhanh
    public float rotationSmoothTime = 0.1f;
    public float gravity = -9.81f;

    [Header("Thành phần")]
    public Animator animator;          // Animator của nhân vật
    public Transform cameraTransform;  // Gán Main Camera vào đây (hoặc tự động tìm)

    private CharacterController controller;
    private InputSystem_Actions inputActions;

    private Vector2 moveInput;
    private bool isSprinting;
    private float turnSmoothVelocity;
    private Vector3 velocity;
    private float speed;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        controller = GetComponent<CharacterController>();

        // 🔧 Nếu quên gán camera, tự động tìm Main Camera
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("[PlayerController] Đã tự động gán Main Camera vào cameraTransform.");
        }

        // 🔧 Nếu quên gán Animator, tự động tìm trong object con
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
                Debug.LogWarning("[PlayerController] Chưa tìm thấy Animator! Hãy gán thủ công trong Inspector.");
        }
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        // Di chuyển
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Sprint
        inputActions.Player.Sprint.performed += ctx => isSprinting = true;
        inputActions.Player.Sprint.canceled += ctx => isSprinting = false;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        Move();
        ApplyGravity();
        UpdateAnimator();
    }

    void Move()
    {
        // Nếu chưa có cameraTransform thì không xoay theo camera
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (cameraTransform != null)
                targetAngle += cameraTransform.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Tốc độ tùy theo trạng thái
            if (isSprinting)
                speed = sprintSpeed;
            else if (moveInput.magnitude > 0.5f)
                speed = runSpeed;
            else
                speed = walkSpeed;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            speed = 0f;
        }
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsSprinting", isSprinting);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }
}
