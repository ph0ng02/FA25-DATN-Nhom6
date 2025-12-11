using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damage = 10f;  // float để đúng với TakeDamage(float)

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem object có implement IDamageable không
        IDamageable dmg = other.GetComponent<IDamageable>();

        if (dmg != null)
        {
            dmg.TakeDamage(damage);   // Gây damage vào Player Health script của bạn
            // Debug.Log("Enemy VFX gây damage: " + damage);
        }
    }
}
