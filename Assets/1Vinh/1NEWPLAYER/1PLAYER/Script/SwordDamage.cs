using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] private float damage = 20f;

    private bool canDamage = false;  // chỉ gây damage khi hoạt ảnh chém đang active

    public void EnableDamage()  // gọi từ Animation Event
    {
        canDamage = true;
    }

    public void DisableDamage() // gọi từ Animation Event
    {
        canDamage = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canDamage) return;

        // Lấy component có IDamageable
        IDamageable target = collision.collider.GetComponent<IDamageable>();
        if (target != null)
        {
            Debug.Log("⚔ Kiếm va chạm và gây damage!");
            target.TakeDamage(damage);
        }
    }
}
