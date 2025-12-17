using UnityEngine;

public class DamageOverTimeTrigger : MonoBehaviour
{
    public int damagePerSecond = 2;

    private bool playerInside;
    private PlayerHealth playerHealth;

    void Update()
    {
        if (playerInside && playerHealth != null)
        {
            playerHealth.TakeDamage(
                Mathf.RoundToInt(damagePerSecond * Time.deltaTime),
                0f,
                Vector3.zero
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerHealth = other.GetComponent<PlayerHealth>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerHealth = null;
        }
    }
}
