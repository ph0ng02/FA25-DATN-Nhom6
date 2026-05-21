using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;   // ❗ CHỈ PLAYER MỚI XỬ LÝ

        if (other.TryGetComponent(out IDamageable target))
        {
            Debug.Log("ENTER HIT: " + other.name);
            target.TakeDamage(damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;   // ❗ CHỈ PLAYER

        if (other.TryGetComponent(out IDamageable target))
        {
            Debug.Log("STAY HIT: " + other.name);
            target.TakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("EXIT: " + other.name);
    }
}
