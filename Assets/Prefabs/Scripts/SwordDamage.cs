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


    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;
   
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            Debug.Log("⚔ Kiếm hit enemy!");
            target.TakeDamage(damage);
        }
    }
}
