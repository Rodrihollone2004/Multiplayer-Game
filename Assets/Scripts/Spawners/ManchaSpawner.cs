using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ManchaSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] playerSpawns;
    [SerializeField] GameObject manchaSpawn;

    void Start()
    {
        LobbySpawner.Instance.ClearSpawnedPlayers();

        if (PhotonNetwork.IsMasterClient)
        {
            int randomMancha = Random.Range(1, PhotonNetwork.CurrentRoom.PlayerCount + 1);
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "ManchaID", randomMancha }
            });
        }

        StartCoroutine(SpawnAfterDelay());
    }

    IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        int manchaID = (int)PhotonNetwork.CurrentRoom.CustomProperties["ManchaID"];

        string prefabToSpawn = "Player";
        switch (actorNumber)
        {
            case 1: prefabToSpawn = "Player"; break;
            case 2: prefabToSpawn = "Player 2"; break;
            case 3: prefabToSpawn = "Player 3"; break;
            case 4: prefabToSpawn = "Player 4"; break;
        }

        Vector3 spawnPos;

        if (actorNumber == manchaID)
        {
            spawnPos = manchaSpawn.transform.position;
        }
        else
        {
            int spawnIndex = actorNumber - 1;
            spawnPos = playerSpawns[Mathf.Clamp(spawnIndex, 0, playerSpawns.Length - 1)].transform.position;
        }

        GameObject player = PhotonNetwork.Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        Debug.Log($"Jugador {actorNumber} spawneado en {(actorNumber == manchaID ? "MANCHA" : "normal")} spawn.");
    }
}
