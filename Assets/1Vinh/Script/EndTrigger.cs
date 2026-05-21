using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelEndManager.Instance != null)
                LevelEndManager.Instance.SetPlayerReady(true);
            else
                Debug.LogError("❌ LevelEndManager.Instance bị null!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelEndManager.Instance != null)
                LevelEndManager.Instance.SetPlayerReady(false);
        }
    }
}
