using UnityEngine;

public class DoorControll : MonoBehaviour
{
    public float openHeight = 3f;    // khoảng cách cửa mở lên
    public float speed = 2f;         // tốc độ di chuyển

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.up * openHeight;
    }

    void Update()
    {
        // Di chuyển cửa mượt giữa 2 trạng thái
        Vector3 target = isOpen ? openPos : closedPos;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }
}
