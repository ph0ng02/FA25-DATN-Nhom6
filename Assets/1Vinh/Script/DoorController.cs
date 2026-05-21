using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform door;
    public float openHeight = 3f;
    public float openSpeed = 2f;

    [HideInInspector] public bool isOpen = false;

    private Vector3 closedPos;
    private Vector3 openPos;

    void Start()
    {
        if (door == null) door = transform; // tự động lấy chính object đang gắn script
        closedPos = door.position;
        openPos = door.position + Vector3.up * openHeight;
    }

    void Update()
    {
        Vector3 target = isOpen ? openPos : closedPos;
        door.position = Vector3.MoveTowards(door.position, target, openSpeed * Time.deltaTime);
    }
}
