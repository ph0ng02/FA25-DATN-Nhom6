using UnityEngine;

public class DamageOverTimeTrigger : MonoBehaviour
{
    [Header("Damage Over Time Settings")]
    public float damagePerSecond = 50f;  // Số máu sẽ mất mỗi giây
    public float damageDuration = 10f;    // Thời gian mất máu trong trigger

    private bool isPlayerInZone = false;
    private HealthManagement playerHealth;  // Lưu tham chiếu đến HealthManagement của Player

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<HealthManagement>();
            if (playerHealth != null)
            {
                Debug.Log("Player entered the damage zone.");
                isPlayerInZone = true;
                StartCoroutine(DealDamageOverTime());
            }
            else
            {
                Debug.LogError("Player does not have HealthManagement script attached.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the damage zone.");
            isPlayerInZone = false;
        }
    }

    private System.Collections.IEnumerator DealDamageOverTime()
    {
        float timeElapsed = 0f;

        while (isPlayerInZone && timeElapsed < damageDuration)
        {
            if (playerHealth != null)
            {
                Debug.Log("Dealing damage: " + damagePerSecond * Time.deltaTime);
                playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
            timeElapsed += Time.deltaTime;
            yield return null;  // Chờ cho đến frame tiếp theo
        }
    }
}
