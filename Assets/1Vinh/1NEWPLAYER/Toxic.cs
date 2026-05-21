using UnityEngine;
using TMPro;
using System.Collections;

public class Toxic : MonoBehaviour
{
    [Header("Sát thương")]
    [SerializeField] private float damageOnContact = 99f;
    [SerializeField] private float damageCooldown = 0.5f;

    [Header("UI Cảnh báo")]
    [SerializeField] private GameObject warningText; // Gán Text (TMP) vào đây

    private float lastDamageTime = 0f;
    private IDamageable targetInside;
    private Coroutine blinkRoutine;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            targetInside = target;
            Debug.Log("⚠ Vào vùng độc: " + other.name);

            // Gây damage ngay lần đầu
            targetInside.TakeDamage(damageOnContact);
            lastDamageTime = Time.time;

            if (warningText != null)
            {
                // Bật cảnh báo và bắt đầu chớp tắt
                if (blinkRoutine == null)
                    blinkRoutine = StartCoroutine(BlinkWarning());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable exiting = other.GetComponentInParent<IDamageable>();
        if (exiting != null && exiting == targetInside)
        {
            targetInside = null;

            // Dừng chớp tắt
            if (blinkRoutine != null)
            {
                StopCoroutine(blinkRoutine);
                blinkRoutine = null;
            }

            if (warningText != null)
                warningText.SetActive(false);

            Debug.Log("✅ Ra khỏi vùng độc: " + other.name);
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

    private IEnumerator BlinkWarning()
    {
        while (true)
        {
            warningText.SetActive(true);
            yield return new WaitForSeconds(0.3f); // thời gian sáng
            warningText.SetActive(false);
            yield return new WaitForSeconds(0.3f); // thời gian tắt
        }
    }
}
