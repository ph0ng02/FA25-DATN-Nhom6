using UnityEngine;

public class CameraFollow222 : MonoBehaviour
{
    // Đối tượng mà camera sẽ theo dõi (Target)
    public Transform target;

    // Khoảng cách ban đầu giữa camera và target
    private Vector3 offset;

    // Tốc độ di chuyển mượt mà của camera
    public float smoothSpeed = 0.125f;

    void Start()
    {
        // 1. Tính toán khoảng cách (offset) ban đầu 
        // Lấy vị trí của camera trừ đi vị trí của target
        offset = transform.position - target.position;
    }

    // Dùng LateUpdate để đảm bảo camera di chuyển sau khi target đã di chuyển
    void LateUpdate()
    {
        // 2. Tính toán vị trí mong muốn của camera
        // Vị trí Target + Khoảng cách Offset
        Vector3 desiredPosition = target.position + offset;

        // 3. Di chuyển camera một cách mượt mà (Lerp)
        // Lerp là hàm nội suy tuyến tính, giúp chuyển động mượt mà hơn
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Gán vị trí đã làm mượt cho camera
        transform.position = smoothedPosition;

        // Tùy chọn: Để camera luôn hướng vào target
        transform.LookAt(target);
    }
}