using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource audioSource;
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Khi ở Menu → phát nhạc Menu
        if (SceneManager.GetActiveScene().name == "MainMenu")
            PlayMusic(menuMusic);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nếu scene là Menu → chơi nhạc menu
        if (scene.name == "MainMenu")
        {
            PlayMusic(menuMusic);
        }
        // Nếu scene là Cutscene → KHÔNG chơi nhạc
        else if (scene.name == "CutSceneOpening")
        {
            StopMusic(); // hoặc không làm gì
        }
        // Nếu scene là Map đầu tiên → chơi nhạc game
        else if (scene.name == "MapStart")
        {
            PlayMusic(gameMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
}
