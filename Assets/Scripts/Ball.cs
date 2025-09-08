using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviourPunCallbacks
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void PickUp(int playerViewID)
    {
        photonView.RPC("RPC_PickUp", RpcTarget.All, playerViewID);
    }

    [PunRPC]
    void RPC_PickUp(int playerViewID)
    {
        GameObject playerObj = PhotonView.Find(playerViewID).gameObject;
        Transform holdPoint = playerObj.GetComponent<PlayerBallHandler>().HoldPoint;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;

        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerObj.GetComponent<Collider2D>(), true);
    }

    public void Throw(Vector2 dir, int playerViewID, float speed)
    {
        photonView.RPC("RPC_Throw", RpcTarget.All, dir.x, dir.y, playerViewID, speed);
    }

    [PunRPC]
    void RPC_Throw(float dirX, float dirY, int playerViewID, float speed)
    {
        Vector2 dir = new Vector2(dirX, dirY);

        GameObject playerObj = PhotonView.Find(playerViewID).gameObject;
        Collider2D playerCol = playerObj.GetComponent<Collider2D>();
        Collider2D ballCol = GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(ballCol, playerCol, true);

        transform.SetParent(null);
        rb.isKinematic = false;
        rb.velocity = dir * speed;

        StartCoroutine(ReenableCollision(ballCol, playerCol, 0.2f));
    }

    IEnumerator ReenableCollision(Collider2D ballCol, Collider2D playerCol, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(ballCol, playerCol, false);
    }
}
