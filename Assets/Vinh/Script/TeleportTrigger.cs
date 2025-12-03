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
        Debug.Log("Object entered: " + other.name);

        if (other.CompareTag(player1Tag))
        {
            player1Inside = true;
            Debug.Log("Player 1 entered");
        }
        else if (other.CompareTag(player2Tag))
        {
            player2Inside = true;
            Debug.Log("Player 2 entered");
        }
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
            // Disable controller trước khi dịch chuyển
            CharacterController c1 = player1.GetComponent<CharacterController>();
            CharacterController c2 = player2.GetComponent<CharacterController>();
            if (c1 != null) c1.enabled = false;
            if (c2 != null) c2.enabled = false;

            // Dịch chuyển
            player1.transform.position = teleportDestination.position + new Vector3(-1f, 0f, 0f);
            player2.transform.position = teleportDestination.position + new Vector3(1f, 0f, 0f);

            // Bật lại controller
            if (c1 != null) c1.enabled = true;
            if (c2 != null) c2.enabled = true;
        }

        Debug.Log("✅ Teleported both players!");
        player1Inside = false;
        player2Inside = false;
    }
    void Update()
    {
        if (player1Inside && player2Inside)
        {
            TeleportPlayers();
        }
    }
}
