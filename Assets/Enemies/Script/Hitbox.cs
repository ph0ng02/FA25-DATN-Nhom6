using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 10f;
    public LayerMask hitLayers;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitLayers) == 0) return;

        var dmg = other.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }
    }
}