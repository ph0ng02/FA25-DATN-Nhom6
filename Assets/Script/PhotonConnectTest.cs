using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnectTest : MonoBehaviourPunCallbacks
{
    bool isConnected = false;
    string roomName = "TestRoom";

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("🔌 Connecting to Photon...");
        }
        else
        {
            isConnected = true;
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✅ Connected to Photon Master Server!");
        isConnected = true;
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        if (!isConnected)
        {
            Debug.LogWarning("⚠️ Not connected yet! Wait for connection to Master Server...");
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(roomName, options);
        Debug.Log("🚀 Creating room: " + roomName);
    }

    public void JoinRoom()
    {
        if (!isConnected)
        {
            Debug.LogWarning("⚠️ Not connected yet! Wait for connection to Master Server...");
            return;
        }

        PhotonNetwork.JoinRoom(roomName);
        Debug.Log("🎮 Joining room: " + roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"🎉 Joined room '{roomName}' successfully! Players in room: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("🗺️ Host detected — loading GameplayScene...");
            PhotonNetwork.LoadLevel("GameplayScene");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"❌ Join room failed: {message}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"❌ Create room failed: {message}");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"⚡ Disconnected from Photon: {cause}");
        isConnected = false;
    }
}
