using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Player mà camera sẽ theo dõi
    public Transform cameraPivot; // Điểm gắn camera (thường nằm sau đầu nhân vật)

    [Header("Camera Settings")]
    public float distance = 3.5f;       // Khoảng cách từ camera đến player
    public float height = 1.8f;         // Độ cao của camera so với player
    public float rotationSpeed = 5f;    // Tốc độ xoay camera theo chuột

    private float yaw; // Góc xoay ngang
    private float pitch; // Góc xoay dọc

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (player == null)
        {
            Debug.LogError("Chưa gán Player cho ThirdPersonCamera!");
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Nhận input chuột
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Xoay góc nhìn
        yaw += mouseX * rotationSpeed;
        pitch -= mouseY * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -35f, 60f); // Giới hạn góc nhìn lên/xuống

        // Xoay quanh player
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, height, -distance);
        transform.position = player.position + offset;
        transform.LookAt(player.position + Vector3.up * height * 0.5f);

        // Player xoay theo hướng camera (nếu bạn muốn)
        player.rotation = Quaternion.Euler(0, yaw, 0);
    }
}
