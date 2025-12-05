using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public Transform spawnPoint;   // Kéo spawn point vào đây

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset vị trí Player
            other.transform.position = spawnPoint.position;

            // Nếu Player có Rigidbody thì reset luôn vận tốc
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
