using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController_Gamepad : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;           // tốc độ đi bộ
    public float runSpeed = 10f;           // tốc độ khi nhấn nút chạy
    public float rotationSpeed = 90f;      // tốc độ xoay chậm hơn
    public float gravity = -9.81f;         // trọng lực
    public float jumpHeight = 2f;          // chiều cao nhảy

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public Animator animator;              // gán Animator trong Inspector

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

        // --- Nhận input từ tay cầm ---
        float horizontal = Input.GetAxis("Horizontal2"); // Xoay nhân vật
        float vertical = Input.GetAxis("Vertical2");     // Tiến/lùi

        // --- Xoay nhân vật bằng cần trái (trái/phải) ---
        transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);

        // --- Kiểm tra nút chạy ---
        bool isRunning = Input.GetKey(KeyCode.JoystickButton0); // Nút A để chạy (đổi nếu cần)
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // --- Di chuyển tiến/lùi ---
        Vector3 move = transform.forward * vertical * currentSpeed;
        controller.Move(move * Time.deltaTime);

        // --- Cập nhật Animator ---
        animator.SetFloat("Speed", Mathf.Abs(vertical) * currentSpeed, 0.1f, Time.deltaTime);
        animator.SetBool("isRunning", isRunning);

        // --- Nhảy ---
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && isGrounded) // B button để nhảy
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        // --- Áp dụng trọng lực ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
