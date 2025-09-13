using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviourPunCallbacks
{
    Rigidbody2D rb;
    public bool IsHeld { get; private set; } = false;
    public bool CanCauseDamage { get; set; } = false;
    public Rigidbody2D Rb { get => rb; set => rb = value; }

    private float lastHitTime = -1f;
    private float damageCooldown = 0.8f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
        {
            CanCauseDamage = false;
            return;
        }

        var health = col.gameObject.GetComponent<PlayerHealth>();

        if (!photonView.IsMine) return;

        if (health != null && CanCauseDamage && rb.velocity.magnitude > 5f)
        {
            if (Time.time - lastHitTime > damageCooldown)
            {
                lastHitTime = Time.time;
                health.GetHit();

                photonView.RPC("RPC_ResetBall", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void RPC_ResetBall()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        CanCauseDamage = false; // Ya no puede causar daño después de golpear
    }

    public void PickUp(int playerViewID)
    {
        photonView.RPC("RPC_PickUp", RpcTarget.All, playerViewID);
    }

    [PunRPC]
    void RPC_PickUp(int playerViewID)
    {
        IsHeld = true;
        CanCauseDamage = false;

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
        IsHeld = false;
        CanCauseDamage = true; // Ahora puede causar daño

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

    // Método público para verificar si se puede agarrar
    public bool CanBePickedUp()
    {
        return !IsHeld && !CanCauseDamage;
    }

    [PunRPC]
    void RPC_Parry(float x, float y)
    {
        Vector2 repelDir = new Vector2(x, y);
        rb.AddForce(repelDir * 20f, ForceMode2D.Impulse);
        CanCauseDamage = false; 
    }

}