using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    private Vector3 startPosition;   // Lưu vị trí ban đầu
    private Quaternion startRotation; // Lưu hướng ban đầu

    void Start()
    {
        // Ghi nhớ vị trí + hướng ban đầu của object
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void Respawn()
    {
        // Đưa object về lại vị trí ban đầu
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
