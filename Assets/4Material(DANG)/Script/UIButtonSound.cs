using UnityEngine;
using UnityEngine.EventSystems; // Cần thiết để bắt sự kiện di chuột

// Thêm IPointerEnterHandler để bắt sự kiện chuột đi vào
public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    // Hàm này được gọi tự động khi con trỏ chuột di chuyển vào khu vực của nút
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Chuột Vừa Di Chuyển Qua Nút!"); // <--- THÊM DÒNG NÀY
        
        // Kiểm tra xem Manager có tồn tại không và gọi hàm phát âm thanh
        if (UISoundManager.Instance != null)
        {
            // Gọi hàm PlayHoverSound() từ script Manager duy nhất
            UISoundManager.Instance.PlayHoverSound();
        }
    }
    
    // (Bạn có thể thêm IPointerClickHandler để phát âm thanh click nếu cần)
}