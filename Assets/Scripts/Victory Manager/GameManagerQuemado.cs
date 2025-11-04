using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManagerQuemado : MonoBehaviourPunCallbacks
{
    public static GameManagerQuemado Instance;

    [Header("UI de Victoria")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text victoryMessage;

    private void Awake() => Instance = this;

    public void CheckRemainingPlayers()
    {

        var players = FindObjectsOfType<PlayerHealth>();
        var alivePlayers = players.Where(p => !p.IsEliminated).ToList();


        if (alivePlayers.Count == 1)
        {
            string winnerName = alivePlayers[0].photonView.Owner.NickName;
            PhotonView.Get(Instance).RPC("RPC_DeclareWinner", RpcTarget.All, winnerName);

        }
    }

    public void DeclareWinner(string winnerName)
    {
        photonView.RPC("RPC_DeclareWinner", RpcTarget.All, winnerName);
    }

    [PunRPC]
    void RPC_DeclareWinner(string winnerName)
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (victoryMessage != null)
            victoryMessage.text = $"¡{winnerName} GANÓ LA PARTIDA!";

        if (PhotonNetwork.IsMasterClient)
        {
            GlobalGameManager.Instance.AddPoints(winnerName, 1);
            GlobalGameManager.Instance.ReturnToLobby(5f);
        }
    }
}
