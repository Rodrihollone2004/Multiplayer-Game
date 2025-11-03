using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GlobalGameManager : MonoBehaviourPunCallbacks
{
    public static GlobalGameManager Instance;

    private Dictionary<string, int> playerPoints = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var player in PhotonNetwork.PlayerList)
            {
                playerPoints[player.NickName] = 0;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(string playerName, int amount)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("RPC_AddPoints", RpcTarget.All, playerName, amount);
    }

    [PunRPC]
    void RPC_AddPoints(string playerName, int amount)
    {
        if (!playerPoints.ContainsKey(playerName))
            playerPoints[playerName] = 0;
        playerPoints[playerName] += amount;
    }

    public Dictionary<string, int> GetPlayerPoints()
    {
        return new Dictionary<string, int>(playerPoints);
    }
}
