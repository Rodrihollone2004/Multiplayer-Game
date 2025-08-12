using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNameDisplay : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject canvasName;
    [SerializeField] private TMP_Text nameText;

    void Start()
    {
        if (!photonView.IsMine)
        {
            canvasName.SetActive(true);
            nameText.text = photonView.Controller.NickName.ToUpper();
        }
    }
}
