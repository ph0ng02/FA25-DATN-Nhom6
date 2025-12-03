using UnityEngine;

public class BossMusicTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            hasTriggered = true;
            Debug.Log("Entered boss area - switching music!");
            AudioManager1.Instance.PlayBossMusic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Debug.Log("Left boss area - back to normal music");
            AudioManager1.Instance.PlayNormalMusic();
            hasTriggered = false;
        }
    }
}
