using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndManager : MonoBehaviour
{
    public static LevelEndManager Instance;

    [Header("Tên Scene kế tiếp")]
    public string nextSceneName = "MapCutscreen";

    private bool playerReady = false; // 👉 CHỈ 1 PLAYER

    private void Awake()
    {
        Instance = this;
    }

    // 👉 Hàm gọi từ trigger, chỉ cần 1 player là đủ
    public void SetPlayerReady(bool isReady)
    {
        playerReady = isReady;

        Debug.Log($"Player Ready: {playerReady}");

        if (playerReady)
        {
            Debug.Log("Player đã vào vùng → Chuyển sang map tiếp theo");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
