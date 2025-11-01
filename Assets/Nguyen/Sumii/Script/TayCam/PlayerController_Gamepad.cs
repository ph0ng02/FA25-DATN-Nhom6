using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController_Gamepad : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection;
    private bool isRolling = false;
    private bool isRunning = false;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float runSpeed = 6f;
    public float rollSpeed = 8f;
    public float gravity = -9.81f;

    private float yVelocity;

    [Header("Combo Settings")]
    private int comboStep = 0;
    private float lastComboTime;
    public float comboResetTime = 1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleComboReset();
    }

    void HandleMovement()
    {
        // Di chuyển bằng joystick trái
        float h = Gamepad.current.leftStick.x.ReadValue();
        float v = Gamepad.current.leftStick.y.ReadValue();

        Vector3 input = new Vector3(h, 0, v);
        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 move = transform.TransformDirection(input);

        // Kiểm tra chạy (RB1 hoặc RB2)
        if (Gamepad.current.rightShoulder.isPressed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        float speed = isRunning ? runSpeed : moveSpeed;

        // Lăn (roll) khi nhấn RB2
        if (Gamepad.current.rightTrigger.wasPressedThisFrame && !isRolling)
        {
            StartCoroutine(Roll(move));
            return;
        }

        if (controller.isGrounded)
        {
            yVelocity = 0f;
        }

        yVelocity += gravity * Time.deltaTime;
        moveDirection = move * speed;
        moveDirection.y = yVelocity;

        controller.Move(moveDirection * Time.deltaTime);

        // Animation di chuyển
        animator.SetFloat("Speed", move.magnitude * speed);
    }

    System.Collections.IEnumerator Roll(Vector3 direction)
    {
        isRolling = true;
        animator.SetTrigger("Roll");

        float rollTime = 0.5f;
        float elapsed = 0f;

        while (elapsed < rollTime)
        {
            controller.Move(direction * rollSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isRolling = false;
    }

    void HandleComboReset()
    {
        if (Time.time - lastComboTime > comboResetTime)
        {
            comboStep = 0;
        }
    }

    void LateUpdate()
    {
        // Tấn công: nút A
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            animator.SetTrigger("Attack");
        }

        // Combo: nhấn X hoặc Y (3 lần)
        if (Gamepad.current.buttonWest.wasPressedThisFrame || Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            comboStep++;
            lastComboTime = Time.time;

            if (comboStep == 1) animator.SetTrigger("Combo1");
            else if (comboStep == 2) animator.SetTrigger("Combo2");
            else if (comboStep == 3)
            {
                animator.SetTrigger("Combo3");
                comboStep = 0;
            }
        }

        // Tương tác: nút B
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            Interact();
        }
    }

    void Interact()
    {
        // Giả lập tương tác (sau này bạn có thể gắn vào trigger)
        Debug.Log("Tương tác với NPC hoặc vật phẩm!");
        animator.SetTrigger("Interact");
    }
}
