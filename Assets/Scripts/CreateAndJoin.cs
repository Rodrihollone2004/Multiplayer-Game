using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField input_Create;
    [SerializeField] TMP_InputField input_Join;

    public void CreateRoom()
    {
        if (!TryGetRoomName(input_Create, out string roomName))
            return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.EmptyRoomTtl = 100;
        roomOptions.PlayerTtl = 100000;
        roomOptions.BroadcastPropsChangeToAll = true;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom()
    {
        if (!TryGetRoomName(input_Join, out string roomName))
            return;

        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRoomInList(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ErrorUI.Instance.ShowError(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ErrorUI.Instance.ShowError(message);
    }

    private bool TryGetRoomName(TMP_InputField inputField, out string roomName)
    {
        roomName = inputField.text.Trim();

        if (string.IsNullOrEmpty(roomName))
        {
            ErrorUI.Instance.ShowError("The room name cannot be empty.");
            return false;
        }
        return true;
    }
}
