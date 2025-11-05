using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [Header("ID của player (1 hoặc 2)")]
    public int playerID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player" + playerID))
        {
            LevelEndManager.Instance.SetPlayerReady(playerID, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player" + playerID))
        {
            LevelEndManager.Instance.SetPlayerReady(playerID, false);
        }
    }
}
