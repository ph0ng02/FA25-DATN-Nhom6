using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonConnectTest : MonoBehaviour
{
    [SerializeField] private string sceneName = "MapStart";

    void Start()
    {
        Debug.Log("🚀 Game khởi động — sẵn sàng bắt đầu!");
    }

    public void StartGame()
    {
        Debug.Log("🎮 Bắt đầu game — load scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
