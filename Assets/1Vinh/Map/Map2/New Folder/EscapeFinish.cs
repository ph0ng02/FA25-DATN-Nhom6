using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeFinish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Map1");
        }
    }
}
