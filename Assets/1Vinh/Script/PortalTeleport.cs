using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTeleport : MonoBehaviour
{
    public string targetSceneName; // Tên scene muốn load qua
    public Transform targetLocation; // Nếu muốn dịch chuyển trong cùng 1 scene

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Teleport sang scene khác
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                SceneManager.LoadScene(targetSceneName);
            }
            // 2. Hoặc teleport trong cùng scene
            else if (targetLocation != null)
            {
                other.transform.position = targetLocation.position;
            }
        }
    }
}
