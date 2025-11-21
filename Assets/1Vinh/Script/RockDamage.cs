using UnityEngine;

public class RockDamage : MonoBehaviour
{
    public int damage = 20;
    public float knockbackForce = 5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHP = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHP != null)
            {
                Vector3 direction = (collision.transform.position - transform.position).normalized;

                // Gây dame
                playerHP.TakeDamage(damage, knockbackForce, direction);

                // DEBUG
                Debug.Log("Rock hit Player! Gây dame: " + damage);
            }
            else
            {
                Debug.LogWarning("Rock chạm Player nhưng Player không có PlayerHealth!");
            }
        }
    }
}
