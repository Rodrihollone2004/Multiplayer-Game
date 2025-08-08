using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-8, 8), -1.5f, 0), Quaternion.identity);
        player.GetComponent<PlayerMovement>().mark.SetActive(true);
    }

}
