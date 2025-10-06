using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomList : MonoBehaviourPunCallbacks
{
    [Header("References")]
    [SerializeField] GameObject roomPrefab;
    [SerializeField] Transform contentParent; 
    [SerializeField] CreateAndJoin createAndJoin; 

    private List<GameObject> activeRooms = new List<GameObject>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (GameObject obj in activeRooms)
            Destroy(obj);

        activeRooms.Clear();

        foreach (RoomInfo info in roomList)
        {
            if (info.IsOpen && info.IsVisible && info.PlayerCount >= 0)
            {
                GameObject roomObj = Instantiate(roomPrefab, contentParent);
                RoomUI roomUI = roomObj.GetComponent<RoomUI>();

                roomUI.Setup(info.Name, info.PlayerCount, info.MaxPlayers, createAndJoin.JoinRoomInList);

                activeRooms.Add(roomObj);
            }
        }
    }

    public void RefreshRoomListManually()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
            Invoke(nameof(RejoinLobby), 0.3f);
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    private void RejoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }
}
