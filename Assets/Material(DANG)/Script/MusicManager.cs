using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [Header("Menu Music")]
    public AudioSource menuMusic;

    [Header("Gameplay Music per Map")]
    public AudioSource map1Music;
    public AudioSource map2Music;
    public AudioSource map3Music;
    // 👉 Mì có thể thêm bao nhiêu map cũng được

    private static MusicManager instance;

    void Awake()
    {
        // Chỉ để lại 1 MusicManager duy nhất tồn tại qua các scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        PlayMenuMusic();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name.ToLower();

        StopAllMusic();

        if (sceneName.Contains("menu"))
        {
            PlayMenuMusic();
        }
        else if (sceneName.Contains("map1"))
        {
            map1Music?.Play();
        }
        else if (sceneName.Contains("map2"))
        {
            map2Music?.Play();
        }
        else if (sceneName.Contains("map3"))
        {
            map3Music?.Play();
        }
        else
        {
            map1Music?.Play();
        }
    }

    void StopAllMusic()
    {
        menuMusic?.Stop();
        map1Music?.Stop();
        map2Music?.Stop();
        map3Music?.Stop();
    }

    void PlayMenuMusic()
    {
        if (!menuMusic.isPlaying)
            menuMusic.Play();
    }
}
