using UnityEngine;

public class BossMusicTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Entered boss area - switching music!");

            if (AudioManager1.Instance != null)
                AudioManager1.Instance.PlayBossMusic();
            else
                Debug.LogError("❌ AudioManager1.Instance is NULL!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Left boss area - back to normal music");

            if (AudioManager1.Instance != null)
                AudioManager1.Instance.PlayNormalMusic();

            hasTriggered = false;
        }
    }
}
