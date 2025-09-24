using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Player
    public Vector3 offset;        // Khoảng cách từ Player đến Camera
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Tính vị trí mong muốn
        Vector3 desiredPosition = target.position + offset;

        // Nội suy mượt
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // Luôn nhìn vào Player
        transform.LookAt(target);
    }
}
