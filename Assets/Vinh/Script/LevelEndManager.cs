using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndManager : MonoBehaviour
{
    public static LevelEndManager Instance;

    [Header("Tên Scene kế tiếp")]
    public string nextSceneName = "MapCutscreen";

    private bool player1Ready = false;
    private bool player2Ready = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPlayerReady(int playerID, bool isReady)
    {
        if (playerID == 1)
            player1Ready = isReady;
        else if (playerID == 2)
            player2Ready = isReady;

        Debug.Log($"Player1: {player1Ready}, Player2: {player2Ready}");

        // Khi cả 2 người đều trong vùng trigger
        if (player1Ready && player2Ready)
        {
            Debug.Log("Cả 2 đã sẵn sàng → Sang map 2");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
