using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Lấy scene hiện tại
            string currentScene = SceneManager.GetActiveScene().name;

            // Load lại scene
            SceneManager.LoadScene(currentScene);
        }
    }
}
