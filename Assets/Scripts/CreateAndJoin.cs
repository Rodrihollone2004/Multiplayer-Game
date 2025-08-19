using UnityEngine;
using Photon.Pun;
using TMPro;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField input_Create;
    [SerializeField] TMP_InputField input_Join;

    [Header("Error UI")]
    [SerializeField] GameObject errorPanel;
    [SerializeField] TMP_Text errorMessageText;

    public void CreateRoom()
    {
        string roomName = input_Create.text.Trim();

        if (string.IsNullOrEmpty(roomName))
        {
            ShowError("The room name cannot be empty.");
            return;
        }

        //PhotonNetwork.IsMasterClient;
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom()
    {
        string roomName = input_Join.text.Trim();

        if (string.IsNullOrEmpty(roomName))
        {
            ShowError("The room name cannot be empty.");
            return;
        }

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
        ShowError(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ShowError(message);
    }

    void ShowError(string message)
    {
        errorMessageText.text = message;
        errorPanel.SetActive(true);
    }

    public void HideError()
    {
        errorPanel.SetActive(false);
    }
}
