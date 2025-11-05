using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerKeyboardController : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;
    private PlayerInputActions input;
    private Vector3 moveInput;

    void Awake()
    {
        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.PlayerKeyboard.Enable();
        input.PlayerKeyboard.Move.performed += ctx => OnMove(ctx);
        input.PlayerKeyboard.Move.canceled += ctx => moveInput = Vector3.zero;
    }

    void OnDisable()
    {
        input.PlayerKeyboard.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.z);
        controller.Move(move * speed * Time.deltaTime);
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputVec = ctx.ReadValue<Vector2>();
        moveInput = new Vector3(inputVec.x, 0, inputVec.y);
    }
}
