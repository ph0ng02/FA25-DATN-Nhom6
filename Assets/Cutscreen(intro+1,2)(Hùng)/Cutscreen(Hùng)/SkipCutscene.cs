using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SkipCutscene : MonoBehaviour
{
    [Header("Kéo VideoPlayer của bạn vào đây")]
    public VideoPlayer videoPlayer;

    [Header("Tên Scene sẽ chuyển đến sau khi Skip")]
    public string nextSceneName = "Scene11";

    public void SkipVideo()
    {
        // Nếu video đang phát thì dừng lại
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        // Chuyển sang scene mới
        SceneManager.LoadScene(nextSceneName);
    }
}
