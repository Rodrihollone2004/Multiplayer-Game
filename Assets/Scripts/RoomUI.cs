using System;
using TMPro;
using UnityEngine;

public class RoomUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] TMP_Text playerCountText;

    private Action<string> onJoinRoom;
    private string roomName;

    public void Setup(string roomName, int currentPlayers, int maxPlayers, Action<string> onJoinRoom)
    {
        this.roomName = roomName;
        this.onJoinRoom = onJoinRoom;

        roomNameText.text = roomName;
        playerCountText.text = $"{currentPlayers}/{maxPlayers}";
    }

    public void JoinRoom()
    {
        onJoinRoom?.Invoke(roomName);
    }
}
