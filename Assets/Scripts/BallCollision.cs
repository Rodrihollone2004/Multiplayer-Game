using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class BallCollision : MonoBehaviourPunCallbacks
{
    private TMP_Text messageText;

    private void Start()
    {
        messageText = GameObject.Find("MessageText").GetComponent<TMP_Text>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine) return;

        PhotonView otherView = collision.gameObject.GetComponent<PhotonView>();
        if (otherView != null && otherView.IsMine == false)
        {
            photonView.RPC("NotifyCollision", RpcTarget.All, PhotonNetwork.NickName, otherView.Owner.NickName);
        }
    }

    [PunRPC]
    void NotifyCollision(string player1, string player2)
    {
        // if (player1 == PhotonNetwork.NickName || player2 == PhotonNetwork.NickName) return;

        messageText.text = $"{player1} quemo a {player2}";

        StartCoroutine(DesactivateText());
    }

    private IEnumerator DesactivateText()
    {
        yield return new WaitForSeconds(5f);
    }
}
