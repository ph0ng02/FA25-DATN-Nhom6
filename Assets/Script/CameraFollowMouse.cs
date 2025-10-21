using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 2f, -4f);
    public float followSmoothness = 10f;

    void LateUpdate()
    {
        if (!player) return;

        // Lấy hướng của player để xoay camera phía sau
        Quaternion rotation = Quaternion.Euler(player.eulerAngles.x, player.eulerAngles.y, 0);
        Vector3 desiredPosition = player.position + rotation * offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmoothness * Time.deltaTime);
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
