using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoNextScene : MonoBehaviour
{
    [SerializeField] private float delay = 10f; // thời gian chờ

    private void Start()
    {
        Invoke(nameof(LoadNextScene), delay);
    }

    void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }
}
