using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
            PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);
            Debug.Log("👤 Spawned player at " + randomPos);
        }
    }
}
