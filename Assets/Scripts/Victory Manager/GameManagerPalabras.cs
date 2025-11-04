using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameManagerPalabras : MonoBehaviourPunCallbacks
{
    public static GameManagerPalabras Instance;

    [Header("UI de Victoria")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text victoryMessage;

    private void Awake() => Instance = this;

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
            victoryMessage.text = $"¡{winnerName} ganó la partida!";

        if (PhotonNetwork.IsMasterClient)
        {
            GlobalGameManager.Instance.AddPoints(winnerName, 1);
            GlobalGameManager.Instance.ReturnToLobby(5f);
        }
    }
}
