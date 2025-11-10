using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;       // tốc độ đi bộ
    public float runSpeed = 10f;       // tốc độ khi nhấn Shift
    public float rotationSpeed = 720f; // tốc độ xoay
    public float gravity = -9.81f;     // trọng lực
    public float jumpHeight = 2f;      // chiều cao nhảy

    private CharacterController controller;
    private Vector3 velocity;          // lưu vận tốc rơi
    private bool isGrounded;           // kiểm tra đang đứng trên đất

    public Animator animator;          // Animator (gán trong Inspector)

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
            velocity.y = -2f; // giữ player dính sát mặt đất

        // --- Nhận input ---
        float horizontal = Input.GetAxis("Horizontal1");
        float vertical = Input.GetAxis("Vertical1");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // --- Kiểm tra Shift để chạy ---
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // --- Cập nhật Animator ---
        animator.SetFloat("Speed", direction.magnitude * currentSpeed, 0.1f, Time.deltaTime);
        animator.SetBool("isRunning", isRunning);

        // --- Di chuyển ---
        if (direction.magnitude >= 0.1f)
        {
            // Xoay player theo hướng di chuyển
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Di chuyển ngang
            Vector3 move = transform.forward * currentSpeed * Time.deltaTime;
            controller.Move(move);
        }

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
