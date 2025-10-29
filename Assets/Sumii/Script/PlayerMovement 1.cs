using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float sprintSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

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
        // Check grounded
        isGrounded = controller.isGrounded;
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        animator.SetFloat("InputHorizontal", horizontal);
        animator.SetFloat("InputVertical", vertical);

        // Sprinting
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("IsSprinting", isSprinting);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.Normalize();

            // Choose speed
            if (isSprinting)
                speed = sprintSpeed;
            else if (Input.GetKey(KeyCode.LeftControl))
                speed = walkSpeed;
            else
                speed = runSpeed;

            // Move
            controller.Move(moveDir * speed * Time.deltaTime);

            // Rotate smoothly
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
        else
        {
            speed = 0f;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Update Animator
        float inputMagnitude = direction.magnitude;
        animator.SetFloat("InputMagnitude", inputMagnitude, 0.1f, Time.deltaTime);
        animator.SetFloat("Speed", speed);
    }
}
