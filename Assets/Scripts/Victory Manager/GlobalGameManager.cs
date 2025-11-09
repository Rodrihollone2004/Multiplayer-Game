using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviourPunCallbacks
{
    public static GlobalGameManager Instance;

    private Dictionary<string, int> playerPoints = new Dictionary<string, int>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GlobalGameManager inicializado correctamente en: " + gameObject.scene.name);

        InitializePlayerPoints();
    }

    private void InitializePlayerPoints()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!playerPoints.ContainsKey(player.NickName))
                playerPoints[player.NickName] = 0;
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

    public void ReturnToLobby(float delay = 3f)
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(ReturnToLobbyRoutine(delay));
    }

    private IEnumerator ReturnToLobbyRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        photonView.RPC("RPC_ReturnToLobby", RpcTarget.All);
    }

    [PunRPC]
    void RPC_ReturnToLobby()
    {
        if (LobbySpawner.Instance != null)
            LobbySpawner.Instance.ClearSpawnedPlayers();

        PhotonNetwork.LoadLevel("Lobby");
    }
}
