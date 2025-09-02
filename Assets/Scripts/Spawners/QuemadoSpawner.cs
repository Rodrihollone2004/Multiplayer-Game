using Photon.Pun;
using UnityEngine;

public class QuemadoSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] playerSpawns;
    [SerializeField] GameObject[] ballSpawns;

    void Start()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        if (spawnIndex >= 0 && spawnIndex < playerSpawns.Length)
        {
            GameObject playerSpawn = playerSpawns[spawnIndex];
            GameObject player = PhotonNetwork.Instantiate("Player", playerSpawn.transform.position, Quaternion.identity);
            player.GetComponent<PlayerMovement>().mark.SetActive(true);

            GameObject ballSpawn = ballSpawns[spawnIndex];
            PhotonNetwork.Instantiate("Ball", ballSpawn.transform.position, Quaternion.identity);
        }
    }
}
