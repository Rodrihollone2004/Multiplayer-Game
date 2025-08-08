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
        PhotonNetwork.CreateRoom(input_Create.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(input_Join.text);
    }

    public void JoinRoomInList(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
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
