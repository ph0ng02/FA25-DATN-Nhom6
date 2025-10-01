using Unity.Netcode;
using Unity.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;


public class networkStartUI : MonoBehaviour
{

    void OnGUI()
    {
        // Chi hien UI nEu Dang O scene "SumiPlayer"
        if (SceneManager.GetActiveScene().name != "SumiPlayer")
            return;


        float w = 200f, h = 40f;
        float x = 10f, y = 10f;

        if(!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUI.Button(new Rect(x, y, w, h), "Host")) NetworkManager.Singleton.StartHost();
            if (GUI.Button(new Rect(x, y + h + 10, w, h), "Client")) NetworkManager.Singleton.StartClient();
            if (GUI.Button(new Rect(x, y + 2 * (h + 10), w, h), "Host")) NetworkManager.Singleton.StartServer();

        }
    }
}
