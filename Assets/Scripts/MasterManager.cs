using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class MasterManager : MonoBehaviourPunCallbacks
{
    public static MasterManager Instance;

    [Header("UI")]
    [SerializeField] GameObject configPanel;
    [SerializeField] TMP_Text hintText;

    private bool canOpenPanel = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && canOpenPanel && Input.GetKeyDown(KeyCode.E)) 
        { 
                configPanel.SetActive(!configPanel.activeSelf);
        }
    }

    public void SetCanOpenPanel(bool value)
    {
        canOpenPanel = value; 
        if (hintText != null) 
            hintText.gameObject.SetActive(PhotonNetwork.IsMasterClient && value); 

        if (!value) 
            configPanel.SetActive(false);
    }

    public void Quemado()
    {
        photonView.RPC("RPC_Quemado", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_Quemado()
    {
        if (LobbySpawner.Instance != null)
            LobbySpawner.Instance.ClearSpawnedPlayers();

        PhotonNetwork.LoadLevel("Quemado");
    }

    public void Words()
    {
        photonView.RPC("RPC_Words", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_Words()
    {
        if (LobbySpawner.Instance != null)
            LobbySpawner.Instance.ClearSpawnedPlayers();

        PhotonNetwork.LoadLevel("Words");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient) 
        { 
            if (canOpenPanel && hintText != null) 
                hintText.gameObject.SetActive(true); 
        } 
        else 
        { configPanel.SetActive(false); 
            if (hintText != null) 
                hintText.gameObject.SetActive(false); 
        }
    }
}
