using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController_Gamepad : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;         // tốc độ bình thường
    public float runSpeed = 10f;         // tốc độ khi nhấn nút chạy
    public float rotationSpeed = 720f;   // tốc độ xoay nhân vật

    public Animator animator;            // gán Animator trong Inspector
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Lấy input từ tay cầm (Left Stick)
        float horizontal = Input.GetAxis("Horizontal2");
        float vertical = Input.GetAxis("Vertical2");

        Vector3 inputDir = new Vector3(horizontal, 0, vertical);
        Vector3 direction = inputDir.normalized;
        float magnitude = Mathf.Clamp01(inputDir.magnitude);

        // Kiểm tra nút chạy (ví dụ: nút "Joystick Button 0" tương đương A trên Xbox)
        bool isRunning = Input.GetKey(KeyCode.JoystickButton0);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // --- Set animation ---
        animator.SetFloat("Speed", magnitude * walkSpeed, 0.1f, Time.deltaTime); // giữ Idle/Walk mượt
        animator.SetBool("isRunning", isRunning);                                 // bật Run khi nhấn nút

        if (magnitude >= 0.1f)
        {
            // Xoay player theo hướng cần trái
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Di chuyển player
            controller.Move(direction * currentSpeed * Time.deltaTime);
        }
    }
}
