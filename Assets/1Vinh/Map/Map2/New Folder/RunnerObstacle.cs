using UnityEngine;

public class RunnerObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
    }
}
