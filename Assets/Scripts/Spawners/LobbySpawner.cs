using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class LobbySpawner : MonoBehaviourPunCallbacks
{
    public static LobbySpawner Instance;

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
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        string prefabToSpawn = "Player";

        switch (actorNumber)
        {
            case 1: prefabToSpawn = "Player"; break;
            case 2: prefabToSpawn = "Player 2"; break;
            case 3: prefabToSpawn = "Player 3"; break;
            case 4: prefabToSpawn = "Player 4"; break;
        }

        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), -1.5f, 0);
        GameObject player = PhotonNetwork.Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
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
            if (player != null && player.GetComponent<PhotonView>() != null)
                PhotonNetwork.Destroy(player);
        }

        spawnedPlayers.Clear();
    }

}
