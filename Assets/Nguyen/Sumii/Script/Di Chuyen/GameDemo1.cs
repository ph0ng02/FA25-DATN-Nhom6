using UnityEngine;
using UnityEngine.InputSystem;

public class GameDemo1 : MonoBehaviour
{
    public PlayerInput playerInput;
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            if (gameObject.name.Contains("Gamepad"))
                playerInput.SwitchCurrentActionMap("Player_Gamepad1");
            else
                playerInput.SwitchCurrentActionMap("PlayerKeyboard");
        }
    }

    // ✅ Đảm bảo có public và tham số đúng kiểu
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
    }
}
