using UnityEngine;

// Yêu cầu đối tượng này phải có AudioSource
[RequireComponent(typeof(AudioSource))]
public class MonsterSounds : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Tự động lấy component AudioSource trên cùng đối tượng này
        audioSource = GetComponent<AudioSource>();
    }

    // Hàm này BẮT BUỘC phải là 'public' 
    // để Animation Event có thể thấy và gọi nó
    public void PlayAttackSound()
    {
        // Kiểm tra xem có AudioSource không rồi mới Play
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Có thể thêm các hàm public khác cho các tiếng khác
    // public void PlayFootstepSound()
    // {
    //     // ...
    // }
}