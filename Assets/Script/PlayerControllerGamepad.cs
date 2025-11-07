using UnityEngine;

public class PlayerControllerGamepad : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Camera Settings")]
    public Transform cameraFollow;  // Camera sẽ theo player
    public Vector3 cameraOffset = new Vector3(0, 3f, -5f);

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraFollow == null)
        {
            Debug.LogWarning("Chưa gán cameraFollow!");
        }
    }

    void FixedUpdate()
    {
        // Lấy input từ tay cầm
        float horizontal = Input.GetAxis("Horizontal2");
        float vertical = Input.GetAxis("Vertical2");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Xoay player theo hướng di chuyển
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Di chuyển player
            Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
        }

        // Cập nhật vị trí camera
        if (cameraFollow != null)
        {
            Vector3 targetPos = transform.position + transform.TransformDirection(cameraOffset);
            cameraFollow.position = Vector3.Lerp(cameraFollow.position, targetPos, Time.deltaTime * 5f);
            cameraFollow.LookAt(transform.position + Vector3.up * 1.5f);
        }
    }
}
