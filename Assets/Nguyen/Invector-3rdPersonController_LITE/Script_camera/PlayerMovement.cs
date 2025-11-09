using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;       // tốc độ bình thường
    public float runSpeed = 10f;       // tốc độ khi nhấn Shift
    public float rotationSpeed = 720f;

    private CharacterController controller;
    public Animator animator; // gán Animator trong Inspector

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal1");
        float vertical = Input.GetAxis("Vertical1");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // --- Kiểm tra Shift để chạy ---
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // --- Giữ nguyên Speed cho Walk/Idle ---
        animator.SetFloat("Speed", direction.magnitude * walkSpeed, 0.1f, Time.deltaTime);

        // --- Thêm parameter isRunning cho Run ---
        animator.SetBool("isRunning", isRunning);

        if (direction.magnitude >= 0.1f)
        {
            // Xoay player theo hướng di chuyển
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Di chuyển player
            controller.Move(direction * currentSpeed * Time.deltaTime);
        }
    }
}
