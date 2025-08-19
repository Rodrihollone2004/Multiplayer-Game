using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour
{
    public GameObject panel;

    private void Start()
    {
        ShowPanel();
    }

    void ShowPanel()
    {
        if (PhotonNetwork.IsMasterClient)
            panel.SetActive(true);
    }
}
