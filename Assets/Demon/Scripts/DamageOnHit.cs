using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damage = 10f;  // Float để đúng với TakeDamage(float)

    private void OnTriggerEnter(Collider other)
    {
        // Chỉ gây damage cho object có tag "Player"
        if (other.CompareTag("Player"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);   // Gây damage vào player
                // Debug.Log("Enemy VFX gây damage: " + damage);
            }
        }
    }
}
