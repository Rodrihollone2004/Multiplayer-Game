using Photon.Pun;
using UnityEngine;

public class PalabrasSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] playerSpawns;

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

        int spawnIndex = actorNumber - 1;
        if (spawnIndex >= 0 && spawnIndex < playerSpawns.Length)
        {
            GameObject playerSpawn = playerSpawns[spawnIndex];
            GameObject player = PhotonNetwork.Instantiate(prefabToSpawn, playerSpawn.transform.position, Quaternion.identity);

            GameObject path = GameObject.Find($"Paths/Path_{actorNumber}");
            if (path != null)
            {
                var mover = player.GetComponent<StepMover>();
                mover.SetStepsFromPath(path);
            }

            player.GetComponent<PlayerMovement>().mark.SetActive(true);
        }
    }
}
