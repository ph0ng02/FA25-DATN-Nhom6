using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DieCanvasController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dieCanvas; // Tham chiếu đến CanvasGameover
    [SerializeField] private Button resetButton; // Tham chiếu đến nút ResetButton

    private void Start()
    {
        if (dieCanvas != null)
        {
            dieCanvas.SetActive(false);
            Debug.Log("Die Canvas initialized and set to inactive at " + System.DateTime.Now);
        }
        else
        {
            Debug.LogError("Die Canvas is not assigned in Inspector!");
        }

        if (resetButton != null)
        {
            // Gán sự kiện thủ công trong Start (tùy chọn, có thể bỏ nếu dùng Inspector)
            resetButton.onClick.AddListener(ReloadScene);
            Debug.Log("Reset Button assigned at " + System.DateTime.Now);
        }
        else
        {
            Debug.LogError("Reset Button is not assigned in Inspector!");
        }
    }

    public void ShowDieCanvas()
    {
        if (dieCanvas != null)
        {
            dieCanvas.SetActive(true);
            Debug.Log("Die Canvas activated at " + System.DateTime.Now);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogError("Cannot show Die Canvas: not assigned!");
        }
    }

    // Thay đổi thành public để hiển thị trong Inspector
    public void ReloadScene()
    {
        Time.timeScale = 1f;

        // Lấy tên scene hiện tại và load lại
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        Debug.Log("Scene " + currentSceneName + " reloaded at " + System.DateTime.Now);
    }

}