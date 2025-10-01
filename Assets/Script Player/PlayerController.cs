using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float rollSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Roll Settings")]
    public float rollDuration = 0.5f;
    private bool isRolling = false;
    private float rollTimer;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction rollAction;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        runAction = playerInput.actions["Run"];
        rollAction = playerInput.actions["Roll"];
    }

    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Get input
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;

        // Rolling
        if (rollAction.triggered && !isRolling && isGrounded)
        {
            isRolling = true;
            rollTimer = rollDuration;
        }

        if (isRolling)
        {
            controller.Move(move * rollSpeed * Time.deltaTime);
            rollTimer -= Time.deltaTime;

            if (rollTimer <= 0)
                isRolling = false;
        }
        else
        {
            // Run hoặc Walk
            float speed = runAction.IsPressed() ? runSpeed : walkSpeed;
            controller.Move(move * speed * Time.deltaTime);
        }

        // Jump
        if (jumpAction.triggered && isGrounded && !isRolling)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
