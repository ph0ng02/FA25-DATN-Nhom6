using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneToNextScene : MonoBehaviour
{
    public PlayableDirector director;
    public string nextSceneName;

    void Start()
    {
        if (director != null)
        {
            director.stopped += OnCutsceneFinished;
        }
    }

    void OnCutsceneFinished(PlayableDirector pd)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
