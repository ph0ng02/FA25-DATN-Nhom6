using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    // Kéo vào trong Inspector của EnemyDamageDealer
    public float damageAmount = 10f; 
    private bool hasHit = false; 
    
    // Gắn script này vào Attack Collider (trên xương tay)
    void OnTriggerEnter(Collider other)
    {
        // Đảm bảo chỉ gây sát thương 1 lần cho mỗi đòn đánh
        if (other.CompareTag("Player") && !hasHit)
        {
            // Giả định Player có script PlayerHealth với hàm TakeDamage(float damage)
            // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            // if (playerHealth != null)
            // {
            //     playerHealth.TakeDamage(damageAmount);
            // }
            
            Debug.Log("Sát thương (Timer) đã gây lên Player: " + damageAmount);
            hasHit = true; 
        }
    }
    
    // HÀM BẮT BUỘC: Đặt hàm này vào Animation Event (hoặc gọi thủ công) 
    // để reset trạng thái đã đánh sau mỗi đòn
    public void ResetHit()
    {
        hasHit = false;
    }
}