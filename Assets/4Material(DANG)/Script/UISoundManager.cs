using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    // Để gọi từ các script nút khác
    public static UISoundManager Instance; 
    
    // Nguồn âm thanh dùng để phát clip
    public AudioSource audioSource; 
    
    [Header("UI Audio Clips")]
    // Kéo clip âm thanh 'hover' vào đây trong Inspector
    public AudioClip hoverSound; 
    
    void Awake()
    {
        // Khởi tạo Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm công khai (public) mà các nút sẽ gọi
    public void PlayHoverSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            // Phát clip âm thanh mà không làm gián đoạn các clip khác đang phát
            audioSource.PlayOneShot(hoverSound); 
        }
    }
}