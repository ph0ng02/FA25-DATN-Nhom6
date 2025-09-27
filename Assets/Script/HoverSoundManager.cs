using UnityEngine;

public class HoverSoundManager : MonoBehaviour
{
    public static HoverSoundManager Instance;
    public AudioSource audioSource;
    public AudioClip hoverClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHoverSound()
    {
        if (audioSource != null && hoverClip != null)
        {
            audioSource.PlayOneShot(hoverClip);
        }
    }
}
