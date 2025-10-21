using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerMouse : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Mouse Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 200f;
    public float xRotationLimit = 80f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Ẩn và khóa chuột giữa màn hình
        Cursor.visible = false;
        gameObject.tag = "Player"; // Đảm bảo NPC nhận diện được
    }

    void Update()
    {
        HandleMouseLook();
        MovePlayer();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xRotationLimit, xRotationLimit);

        // Xoay camera lên/xuống
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Xoay player theo trục Y (chuột ngang)
        transform.Rotate(Vector3.up * mouseX);
    }

    void MovePlayer()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
