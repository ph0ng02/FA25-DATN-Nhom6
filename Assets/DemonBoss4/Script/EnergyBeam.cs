using UnityEngine;

public class EnergyBeam : MonoBehaviour
{
    public float damage = 20f;       // Số damage của tia
    public float duration = 2f;      // Thời gian tồn tại

    private void Start()
    {
        // Tự hủy sau duration
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        // Nếu tia rơi xuống đất, có thể kiểm tra radius gây damage
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f); // 0.5f là bán kính tác động
        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }
    }

    // Hoặc nếu bạn muốn dùng trigger
    private void OnTriggerEnter(Collider other)
    {
        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }
    }
}
