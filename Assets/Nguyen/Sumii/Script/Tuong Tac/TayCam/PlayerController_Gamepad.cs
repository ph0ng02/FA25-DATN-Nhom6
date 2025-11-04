using UnityEngine;
using UnityEngine.InputSystem; // ⚡ Bắt buộc cho New Input System

[RequireComponent(typeof(CharacterController))]
public class PlayerController_Gamepad : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rollForce = 8f;
    public float gravity = -9.81f;

    [Header("Combo Settings")]
    public float comboResetTime = 1f;

    private CharacterController controller;
    private Animator animator;
    private PlayerInputActions input;  // lớp sinh ra từ Input System

    private Vector3 moveInput;
    private Vector3 velocity;
    private int comboStep = 0;
    private float lastComboTime;

    private void Awake()
    {
        input = new PlayerInputActions(); // ⚡ Tạo instance từ Input Actions
    }

    private void OnEnable()
    {
        input.Player.Enable();

        // Đăng ký sự kiện
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += ctx => moveInput = Vector3.zero;

        input.Player.Attack.performed += ctx => Attack();
        input.Player.Interact.performed += ctx => Interact();
        input.Player.SkillQ.performed += ctx => ComboAttack();
        input.Player.SkillF.performed += ctx => ComboAttack();
        input.Player.Sprint.performed += ctx => Roll();
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= ctx => moveInput = Vector3.zero;

        input.Player.Disable();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleComboReset();
    }

    // Nhận giá trị di chuyển từ joystick trái
    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputVec = ctx.ReadValue<Vector2>();
        moveInput = new Vector3(inputVec.x, 0, inputVec.y);
    }

    private void HandleMovement()
    {
        Vector3 move = moveInput;
        if (move.magnitude > 1f) move.Normalize();

        bool isRunning = input.Player.Sprint.IsPressed();
        float speed = isRunning ? runSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        // Quay hướng di chuyển
        if (move != Vector3.zero)
            transform.forward = move;

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("Speed", move.magnitude * speed);
    }

    private void Roll()
    {
        Vector3 rollDir = transform.forward * rollForce;
        controller.Move(rollDir * Time.deltaTime);
        animator.SetTrigger("Roll");
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void Interact()
    {
        Debug.Log("Tương tác với NPC hoặc vật phẩm");
        animator.SetTrigger("Interact");
    }

    private void ComboAttack()
    {
        comboStep++;
        lastComboTime = Time.time;

        if (comboStep == 1)
            animator.SetTrigger("Combo1");
        else if (comboStep == 2)
            animator.SetTrigger("Combo2");
        else if (comboStep >= 3)
        {
            animator.SetTrigger("Combo3");
            comboStep = 0;
        }
    }

    private void HandleComboReset()
    {
        if (Time.time - lastComboTime > comboResetTime)
        {
            comboStep = 0;
        }
    }
}
