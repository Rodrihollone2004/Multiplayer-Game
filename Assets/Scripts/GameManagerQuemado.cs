using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerQuemado : MonoBehaviourPunCallbacks
{
    public static GameManagerQuemado Instance;

    private void Awake() => Instance = this;

    public void CheckRemainingPlayers()
    {
        var players = FindObjectsOfType<PlayerHealth>();
        var alivePlayers = players.Where(p => !p.IsEliminated).ToList();

        if (alivePlayers.Count == 1)
        {
            photonView.RPC("RPC_DeclareWinner", RpcTarget.All, alivePlayers[0].photonView.Owner.NickName);
        }
    }

    [PunRPC]
    void RPC_DeclareWinner(string winnerName)
    {
        Debug.Log("El ganador es: " + winnerName);
        // Mostrar UI ganador
        // Luego PhotonNetwork.LoadLevel("Lobby") o siguiente minijuego
    }
}
