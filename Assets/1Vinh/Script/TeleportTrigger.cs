using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportDestination; // nơi cần dịch chuyển tới

    private bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered teleport zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void Update()
    {
        if (playerInside)
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("❌ Không tìm thấy Player!");
            return;
        }

        if (teleportDestination == null)
        {
            Debug.LogError("❌ teleportDestination chưa được gán!");
            return;
        }

        // Tắt controller để không lỗi va chạm khi dịch chuyển
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        // Dịch chuyển player tới vị trí mới
        player.transform.position = teleportDestination.position;

        // Bật lại controller
        if (controller != null) controller.enabled = true;

        Debug.Log("✅ Player teleported!");
        playerInside = false;
    }
}
