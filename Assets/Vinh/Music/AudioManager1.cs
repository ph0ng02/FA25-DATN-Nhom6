using UnityEngine;

public class AudioManager1 : MonoBehaviour
{
    public static AudioManager1 Instance; // singleton

    [Header("Audio Sources")]
    public AudioSource musicSource;

    [Header("Clips")]
    public AudioClip normalBGM;
    public AudioClip bossBGM;

    private void Awake()
    {
        // Đảm bảo chỉ có 1 AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        PlayNormalMusic(); // chạy nhạc thường khi bắt đầu
    }

    public void PlayNormalMusic()
    {
        if (musicSource.clip == normalBGM) return;
        musicSource.clip = normalBGM;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayBossMusic()
    {
        if (musicSource.clip == bossBGM) return;
        musicSource.clip = bossBGM;
        musicSource.loop = true;
        musicSource.Play();
    }
}
