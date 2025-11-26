using UnityEngine;

public class VisionTrigger : MonoBehaviour
{
    public bool playerInVision = false;
    public Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInVision = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInVision = false;
            player = null;
        }
    }
}
