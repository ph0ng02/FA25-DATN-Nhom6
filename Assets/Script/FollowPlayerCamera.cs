using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Gán Player vào đây

    [Header("Camera Settings")]
    public float distance = 4f;   // Khoảng cách camera với player
    public float height = 2f;     // Độ cao của camera
    public float smoothSpeed = 5f; // Độ mượt khi di chuyển

    void LateUpdate()
    {
        if (player == null) return;

        // Tính vị trí camera (luôn ở sau player)
        Vector3 targetPosition = player.position - player.forward * distance + Vector3.up * height;

        // Di chuyển camera mượt mà
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // Camera luôn nhìn về phía player
        transform.LookAt(player.position + Vector3.up * height * 0.5f);
    }
}
