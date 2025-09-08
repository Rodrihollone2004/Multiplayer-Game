using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class LobbySpawner : MonoBehaviourPunCallbacks
{
    public static LobbySpawner Instance;

    [Header("Player Prefab")]
    [SerializeField] private string playerPrefabName = "Player";

    private List<GameObject> spawnedPlayers = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), -1.5f, 0);
        GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawnPos, Quaternion.identity);
        spawnedPlayers.Add(player);

        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm != null)
            pm.mark.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        ClearSpawnedPlayers();
    }

    public void ClearSpawnedPlayers()
    {
        foreach (var player in spawnedPlayers)
        {
            if (player != null)
                PhotonNetwork.Destroy(player);
        }

        spawnedPlayers.Clear();
    }
}
