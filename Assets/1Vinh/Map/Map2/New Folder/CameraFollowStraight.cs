using UnityEngine;

public class CameraFollowStraight : MonoBehaviour
{
    public Transform player;          // Player để camera theo
    public Vector3 offset;            // Khoảng cách cố định giữa camera và player
    public float smoothSpeed = 5f;    // Độ mượt khi camera di chuyển

    void LateUpdate()
    {
        // Vị trí mục tiêu chỉ di chuyển theo Player
        Vector3 targetPos = player.position + offset;

        // Giữ X/Y/Z như offset, chỉ di chuyển theo Player
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        // Camera luôn nhìn theo hướng cố định (không xoay theo Player)
        transform.rotation = Quaternion.Euler(20f, 0f, 0f); // Ví dụ: nghiêng 20° xuống
    }
}
