using UnityEngine;

public class EnergyBeamHit : MonoBehaviour
{
    public float damage = 20f;

    private void OnTriggerEnter(Collider other)
    {
        // Chỉ gây damage cho player
        if (other.CompareTag("Player"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null)
                dmg.TakeDamage(damage);
        }
    }
}
