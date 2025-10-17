using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }

    public void OpenOption()
    {
        SceneManager.LoadScene("OptionScene");
    }

    public void OpenMusic()
    {
        SceneManager.LoadScene("MusicScene");
    }

    public void OpenMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game đã thoát (chỉ hiện trong Editor).");
    }
}
