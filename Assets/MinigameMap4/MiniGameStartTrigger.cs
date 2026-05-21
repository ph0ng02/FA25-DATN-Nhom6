using UnityEngine;

public class MiniGameStartTrigger : MonoBehaviour
{
    public TrapManager trapManager;

    private void OnTriggerEnter(Collider other)
{
    if (!other.CompareTag("Player")) return;

    trapManager.StartMiniGame();
    gameObject.SetActive(false);
}
}
