using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController_Gamepad : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;          // tốc độ đi bộ
    public float runSpeed = 10f;          // tốc độ khi nhấn nút chạy
    public float rotationSpeed = 720f;    // tốc độ xoay nhân vật
    public float gravity = -9.81f;        // trọng lực
    public float jumpHeight = 2f;         // chiều cao nhảy (nếu muốn thêm)

    private CharacterController controller;
    private Vector3 velocity;             // vận tốc rơi
    private bool isGrounded;              // kiểm tra đang đứng trên đất

    public Animator animator;             // gán Animator trong Inspector

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
        {
            velocity.y = -2f; // giữ player dính sát đất, tránh bị lơ lửng
        }

        // --- Input từ tay cầm ---
        float horizontal = Input.GetAxis("Horizontal2");
        float vertical = Input.GetAxis("Vertical2");

        Vector3 inputDir = new Vector3(horizontal, 0, vertical);
        Vector3 direction = inputDir.normalized;
        float magnitude = Mathf.Clamp01(inputDir.magnitude);

        // --- Kiểm tra nút chạy (A hoặc B tuỳ tay cầm) ---
        bool isRunning = Input.GetKey(KeyCode.JoystickButton0); // hoặc đổi sang JoystickButton1 nếu bạn muốn
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // --- Set animation ---
        animator.SetFloat("Speed", magnitude * currentSpeed, 0.1f, Time.deltaTime);
        animator.SetBool("isRunning", isRunning);

        // --- Di chuyển ngang ---
        if (magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            controller.Move(direction * currentSpeed * Time.deltaTime);
        }

        // --- (Tuỳ chọn) Nhảy ---
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
