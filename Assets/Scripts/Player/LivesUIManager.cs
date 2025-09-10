using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LivesUIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform livesPanel;
    [SerializeField] private TMP_Text playerLifePrefab;

    private Dictionary<int, TMP_Text> playerLivesTexts = new Dictionary<int, TMP_Text>();

    void Start()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            AddPlayerUI(p);
        }
    }

    void AddPlayerUI(Player player)
    {
        TMP_Text lifeText = Instantiate(playerLifePrefab, livesPanel);
        lifeText.text = $"{player.NickName}: 3";
        playerLivesTexts[player.ActorNumber] = lifeText;
    }

    public void UpdatePlayerLives(int actorNumber, int newLives)
    {
        if (playerLivesTexts.ContainsKey(actorNumber))
        {
            playerLivesTexts[actorNumber].text = $"{PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName}: {newLives}";
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerUI(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (playerLivesTexts.ContainsKey(otherPlayer.ActorNumber))
        {
            Destroy(playerLivesTexts[otherPlayer.ActorNumber].gameObject);
            playerLivesTexts.Remove(otherPlayer.ActorNumber);
        }
    }
}
