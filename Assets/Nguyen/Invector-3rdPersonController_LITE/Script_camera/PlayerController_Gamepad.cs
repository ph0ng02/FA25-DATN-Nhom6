using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController_Gamepad : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;          // Tốc độ di chuyển
    public float rotationSpeed = 720f;    // Tốc độ xoay nhân vật (độ/giây)

    private CharacterController controller;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Lấy input từ tay cầm (chuẩn Unity Input Manager)
        float horizontal = Input.GetAxis("Horizontal"); // Left Stick X
        float vertical = Input.GetAxis("Vertical");     // Left Stick Y

        Vector3 inputDir = new Vector3(horizontal, 0, vertical);
        float magnitude = Mathf.Clamp01(inputDir.magnitude);

        // Nếu có di chuyển thì xoay player theo hướng đó
        if (magnitude > 0.1f)
        {
            // Xoay hướng nhân vật theo hướng input
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Di chuyển nhân vật
        moveDirection = transform.forward * magnitude * moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
