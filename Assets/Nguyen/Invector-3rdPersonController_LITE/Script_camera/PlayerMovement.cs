using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 40f;  // tốc độ xoay thân bằng A/D
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        // --- Kiểm tra chạm đất ---
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // --- Nhận input ---
        float horizontal = Input.GetAxis("Horizontal1"); // A/D để xoay
        float vertical = Input.GetAxis("Vertical1");     // W/S để tiến lùi

        // --- Xoay bằng A/D ---
        transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);

        // --- Kiểm tra Shift để chạy ---
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // --- Di chuyển tiến/lùi ---
        Vector3 move = transform.forward * vertical * currentSpeed;
        controller.Move(move * Time.deltaTime);

        // --- Cập nhật Animator ---
        animator.SetFloat("Speed", Mathf.Abs(vertical) * currentSpeed, 0.1f, Time.deltaTime);
        animator.SetBool("isRunning", isRunning);

        // --- Nhảy ---
        if (Input.GetButtonDown("Jump1") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump1");
        }

        // --- Áp dụng trọng lực ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
