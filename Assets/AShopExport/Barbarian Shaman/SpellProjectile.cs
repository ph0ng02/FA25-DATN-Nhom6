using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 15f;
    public float damage = 20f; // Sát thương của đạn phép
    public float lifeTime = 5f; // Tự hủy sau 5 giây nếu không trúng
    
    private Rigidbody rb;
    private Vector3 currentVelocity = Vector3.zero; // <--- KHAI BÁO BIẾN MỚI

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Projectile cần Rigidbody!");
            Destroy(gameObject);
            return;
        }

        // Tự hủy sau lifeTime
        Destroy(gameObject, lifeTime);
    }

    void Update() // <--- THÊM HÀM UPDATE NÀY
    {
        // Sử dụng Transform để di chuyển đạn (buộc di chuyển)
        if (currentVelocity != Vector3.zero)
        {
            transform.position += currentVelocity * Time.deltaTime;
        }
    }
    
    // Hàm này được gọi bởi ShamanAI.cs để phóng đạn
    public void Fire(Vector3 targetPosition)
    {
        // Tính toán hướng bay 
        Vector3 direction = (targetPosition - transform.position).normalized;
        
        // Cấp vận tốc: Lưu trữ vận tốc vào biến mới
        currentVelocity = direction * speed; // <--- CẤP VẬN TỐC TẠI ĐÂY

        // Thử áp dụng vận tốc vật lý, nhưng không bắt buộc vì Update đã xử lý
        if (rb != null)
        {
            // Đặt rb.velocity cũng được, nhưng logic Update sẽ ưu tiên.
            rb.linearVelocity = currentVelocity; 
            Debug.Log("Spell Fired with Velocity: " + currentVelocity); 
        }
    }

    // Xử lý va chạm
    void OnTriggerEnter(Collider other)
    {
        // Bỏ comment dòng sát thương nếu PlayerHealth đã được sửa
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                //playerHealth.TakeDamage(damage); 
            }
            
            Debug.Log("Đạn phép trúng Player và gây " + damage + " sát thương.");
        }
        
        // Luôn hủy đạn sau khi va chạm 
        if (!other.CompareTag("Enemy") && !other.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
    }
}