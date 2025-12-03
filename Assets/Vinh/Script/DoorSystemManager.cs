using System.Collections;
using UnityEngine;

public class DoorSystemManager : MonoBehaviour
{
    [Header("References")]
    public Transform door1;
    public Transform door2;
    public LeverSwitch lever1;
    public LeverSwitch lever2;

    [Header("Door Settings")]
    public float openHeight = 3f;
    public float openSpeed = 2f;
    public Vector3 openDirection = Vector3.up; // Cho phép đổi hướng mở dễ dàng

    private bool isDoorOpened = false;

    public void CheckLevers()
    {
        if (lever1 == null || lever2 == null)
        {
            Debug.LogError("❌ Lever reference missing in DoorSystemManager!");
            return;
        }

        if (door1 == null || door2 == null)
        {
            Debug.LogError("❌ Door reference missing in DoorSystemManager!");
            return;
        }

        if (!isDoorOpened && lever1.isActivated && lever2.isActivated)
        {
            Debug.Log("✅ Both levers activated! Opening doors...");
            StartCoroutine(OpenDoor(door1));
            StartCoroutine(OpenDoor(door2));
            isDoorOpened = true;
        }
    }

    private IEnumerator OpenDoor(Transform door)
    {
        Vector3 start = door.position;
        Vector3 target = start + openDirection * openHeight;

        Debug.Log($"🚪 {door.name} moving from {start} to {target}");

        // đảm bảo không bị static
        door.gameObject.isStatic = false;

        while (Vector3.Distance(door.position, target) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, target, openSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log($"✅ {door.name} opened!");
    }
}