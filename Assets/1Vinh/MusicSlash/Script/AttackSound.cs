using UnityEngine;

public class AttackSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip attackClip;

    // Hàm này sẽ được gọi từ Animation Event
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClip);
    }
}
