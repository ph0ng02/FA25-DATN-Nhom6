using UnityEngine;

public class TriggerZoneDoor : MonoBehaviour
{
    public DoorControll door;
    public Animator anim; // gán Animator của cửa vào đây

    private int playerCount = 0;
    private int enemyCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Đếm cả Player1 và Player2
        if (other.CompareTag("Player"))
            playerCount++;

        if (other.CompareTag("Enemy"))
            enemyCount++;

        UpdateDoorState();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerCount--;

        if (other.CompareTag("Enemy"))
            enemyCount--;

        UpdateDoorState();
    }
    public void OpenDoor()
    {
        if (anim != null)
            anim.SetBool("Open", true);
    }
    public void CloseDoor()
    {
        if (anim != null)
            anim.SetBool("Open", false);
    }

    private void UpdateDoorState()
    {
        // Nếu có cả 2 loại → đóng cửa
        if (playerCount > 0 && enemyCount > 0)
        {
            door.CloseDoor();
        }
        else
        {
            // Không ai hoặc chỉ có 1 loại → mở cửa
            door.OpenDoor();
        }
    }
}
