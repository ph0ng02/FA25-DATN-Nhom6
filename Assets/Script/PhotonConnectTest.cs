using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnectTest : MonoBehaviourPunCallbacks
{
    bool isConnected = false;
    string roomName = "EpicRoom";

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("🔌 Đang kết nối tới Photon...");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✅ Đã kết nối tới Photon Master Server!");
        isConnected = true;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("🏠 Đã vào Lobby, có thể tạo hoặc join phòng!");
    }

    public void StartGame()
    {
        if (!isConnected)
        {
            Debug.LogWarning("⚠️ Chưa kết nối tới Master Server!");
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
        Debug.Log("🚀 Đang tạo hoặc tham gia phòng: " + roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"🎉 Đã vào phòng '{roomName}' thành công! Người chơi: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("🗺️ Là chủ phòng — load GameplayScene...");
            PhotonNetwork.LoadLevel("GameplayScene");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"❌ Tạo phòng thất bại: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"❌ Tham gia phòng thất bại: {message}");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"⚡ Mất kết nối tới Photon: {cause}");
        isConnected = false;
    }
}
