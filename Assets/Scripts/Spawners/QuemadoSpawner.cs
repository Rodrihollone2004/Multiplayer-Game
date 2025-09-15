using Photon.Pun;
using UnityEngine;

public class QuemadoSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] playerSpawns;
    [SerializeField] GameObject[] ballSpawns;

    void Start()
    {
        LobbySpawner.Instance.ClearSpawnedPlayers();

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        string prefabToSpawn = "Player";

        switch (actorNumber)
        {
            case 1: prefabToSpawn = "Player"; break;
            case 2: prefabToSpawn = "Player 2"; break;
            case 3: prefabToSpawn = "Player 3"; break;
            case 4: prefabToSpawn = "Player 4"; break;
        }

        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        if (spawnIndex >= 0 && spawnIndex < playerSpawns.Length)
        {
            GameObject playerSpawn = playerSpawns[spawnIndex];
            GameObject player = PhotonNetwork.Instantiate(prefabToSpawn, playerSpawn.transform.position, Quaternion.identity);
            player.GetComponent<PlayerMovement>().mark.SetActive(true);

            GameObject ballSpawn = ballSpawns[spawnIndex];
            PhotonNetwork.Instantiate("Ball", ballSpawn.transform.position, Quaternion.identity);
        }
    }
}
