using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviourPun
{
    [SerializeField] float throwSpeed = 12f;
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
        Transform holdPoint = playerObj.GetComponent<PlayerMovement>().HoldPoint;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;

        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerObj.GetComponent<Collider2D>(), true);
    }
    public void Throw(Vector2 dir, int playerViewID)
    {
        photonView.RPC("RPC_Throw", RpcTarget.AllBuffered, dir.x, dir.y, playerViewID);
    }

    [PunRPC]
    void RPC_Throw(float dirX, float dirY, int playerViewID)
    {
        Vector2 dir = new Vector2(dirX, dirY);

        GameObject playerObj = PhotonView.Find(playerViewID).gameObject;
        Collider2D playerCol = playerObj.GetComponent<Collider2D>();
        Collider2D ballCol = GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(ballCol, playerCol, true);

        transform.SetParent(null);
        rb.isKinematic = false;
        rb.velocity = dir * throwSpeed;

        StartCoroutine(ReenableCollision(ballCol, playerCol, 0.2f));
    }

    IEnumerator ReenableCollision(Collider2D ballCol, Collider2D playerCol, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(ballCol, playerCol, false);
    }
}
