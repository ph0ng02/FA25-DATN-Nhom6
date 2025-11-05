using UnityEngine;
using UnityEngine.InputSystem;

public class GameDemo1 : MonoBehaviour
{
    public PlayerInput playerInput;
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;

    [SerializeField] private bool isGamepadPlayer = false; // chọn loại điều khiển trong Inspector

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Bật đúng Action Map cho player này
        if (playerInput != null)
        {
            if (isGamepadPlayer)
                playerInput.SwitchCurrentActionMap("Player_Gamepad1");
            else
                playerInput.SwitchCurrentActionMap("PlayerKeyboard");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
    }
}
