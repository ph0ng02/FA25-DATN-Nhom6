using UnityEngine;
using UnityEngine.SceneManagement;

public class DieCanvasController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dieCanvas; // Canvas chết (tùy chọn)

    private bool isDead = false;

    public void ShowDieCanvas()
    {
        if (isDead) return;
        isDead = true;

        if (dieCanvas != null)
            dieCanvas.SetActive(true);

        Time.timeScale = 0f; // Tạm dừng game (nếu bạn muốn)

        // Bắt đầu chuyển scene sau 3 giây thời gian thực
        StartCoroutine(ReturnToMenuAfterDelay());
    }

    private System.Collections.IEnumerator ReturnToMenuAfterDelay()
    {
        // Chờ 3 giây trong thời gian thực (không bị ảnh hưởng bởi Time.timeScale = 0)
        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f; // Bật lại timeScale trước khi load scene

        SceneManager.LoadScene("MainMenu"); // ← Đổi "Menu" thành tên scene Menu của bạn
    }
}
