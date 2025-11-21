using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public float damage = 20f;
    public string enemyTag = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Hit Enemy! Damage = " + damage);
            }
        }
    }
}
