using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoCutsceneNext : MonoBehaviour
{
    public VideoPlayer video;
    public string nextSceneName = "MapStart";

    void Start()
    {
        video.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
