using UnityEngine;

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

    private Vector3 velocity;
    private int comboStep = 0;
    private float lastComboTime;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
        HandleInteraction();
        HandleCombo();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");   // Joystick trái
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical);
        if (move.magnitude > 1f) move.Normalize();

        bool isRunning = Input.GetButton("RB1") || Input.GetButton("RB2");

        float speed = isRunning ? runSpeed : walkSpeed;
        controller.Move(move * speed * Time.deltaTime);

        // Quay theo hướng di chuyển
        if (move != Vector3.zero)
            transform.forward = move;

        // Roll
        if (Input.GetButtonDown("RB1") || Input.GetButtonDown("RB2"))
        {
            Vector3 rollDir = transform.forward * rollForce;
            controller.Move(rollDir * Time.deltaTime);
            animator.SetTrigger("Roll");
        }

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("Speed", move.magnitude * speed);
    }

    void HandleAttack()
    {
        if (Input.GetButtonDown("joystick button0")) // A = Attack
        {
            animator.SetTrigger("Attack");
        }
    }

    void HandleInteraction()
    {
        if (Input.GetButtonDown("joystick button1")) // B = Interaction
        {
            Debug.Log("Tương tác với NPC hoặc vật phẩm");
            animator.SetTrigger("Interact");
        }
    }

    void HandleCombo()
    {
        if (Input.GetButtonDown("joystick button2")) // X
        {
            ComboAttack();
        }
        else if (Input.GetButtonDown("joystick button3")) // Y
        {
            ComboAttack();
        }

        if (Time.time - lastComboTime > comboResetTime)
        {
            comboStep = 0;
        }
    }

    void ComboAttack()
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
}
