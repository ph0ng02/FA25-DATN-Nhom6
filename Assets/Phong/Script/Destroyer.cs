using UnityEngine;

public class Destroyer : MonoBehaviour
{
    // Nhớ tích vào "Is Trigger" ở Collider của điểm Z
    private void OnTriggerEnter(Collider other)
    {
        // Nếu cái gì chạm vào vùng này, xóa nó đi
        if (other.CompareTag("Trap")) // Nhớ đặt Tag cho bóng là "Trap"
        {
            Destroy(other.gameObject);
        }
    }
}