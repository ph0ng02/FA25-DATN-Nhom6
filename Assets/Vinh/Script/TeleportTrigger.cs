using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportDestination; // nơi cần dịch chuyển tới
    public string player1Tag = "Player1";
    public string player2Tag = "Player2";

    private bool player1Inside = false;
    private bool player2Inside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(player1Tag))
        {
            player1Inside = true;
        }
        else if (other.CompareTag(player2Tag))
        {
            player2Inside = true;
        }

        CheckBothPlayers();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(player1Tag))
        {
            player1Inside = false;
        }
        else if (other.CompareTag(player2Tag))
        {
            player2Inside = false;
        }
    }

    private void CheckBothPlayers()
    {
        if (player1Inside && player2Inside)
        {
            TeleportPlayers();
        }
    }

    private void TeleportPlayers()
    {
        GameObject player1 = GameObject.FindGameObjectWithTag(player1Tag);
        GameObject player2 = GameObject.FindGameObjectWithTag(player2Tag);

        if (teleportDestination != null)
        {
            // Dịch chuyển cả hai người chơi
            player1.transform.position = teleportDestination.position + new Vector3(-1f, 0f, 0f);
            player2.transform.position = teleportDestination.position + new Vector3(1f, 0f, 0f);
        }

        // Sau khi dịch chuyển, có thể reset lại trạng thái
        player1Inside = false;
        player2Inside = false;
    }
}
