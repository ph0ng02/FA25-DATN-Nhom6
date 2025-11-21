using UnityEngine;

public class GroundFire : MonoBehaviour
{
     [Header("Sát thương")]
    [SerializeField] private float damageOnContact = 999f;
    [SerializeField] private float damageCooldown = 0.5f;

    [Header("UI Cảnh báo")]
    [SerializeField] private GameObject warningText; // Gán Text (TMP) vào đây

    private float lastDamageTime = 0f;
    private IDamageable targetInside;
    private Coroutine warningRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (targetInside == null)
        {
            targetInside = other.GetComponent<IDamageable>();
            if (targetInside != null && warningText != null)
            {
                warningText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable exiting = other.GetComponent<IDamageable>();
        if (exiting != null && exiting == targetInside)
        {
            targetInside = null;
            if (warningText != null)
            {
                warningText.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (targetInside != null)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                Debug.Log("💥 Gây sát thương liên tục trong vùng độc!");
                targetInside.TakeDamage(damageOnContact);
                lastDamageTime = Time.time;
            }
        }
    }
}
