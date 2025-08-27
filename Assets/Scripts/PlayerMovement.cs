using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform holdPoint;

    [Header("Visuals")]
    public GameObject mark;
    Animator anim;
    SpriteRenderer sr;


    Vector2 moveDirection;
    Vector2 lastHorizontalDir = Vector2.right;

    Ball heldBall = null;

    public Transform HoldPoint => holdPoint;
    public Ball HeldBall => heldBall;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            ProcessInputs();
            HandleAnimations();
        }

        if (heldBall != null && Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mousePos - (Vector2)transform.position).normalized;

            heldBall.Throw(dir, photonView.ViewID);

            //StartCoroutine(EnableCollisionAfterThrow(heldBall.GetComponent<Collider2D>(), GetComponent<Collider2D>()));

            heldBall = null;
        }
    }

    private IEnumerator EnableCollisionAfterThrow(Collider2D ballCol, Collider2D playerCol)
    {
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreCollision(ballCol, playerCol, false);
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
            Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if(moveX != 0)
            lastHorizontalDir = new Vector2 (moveX, 0);
    }

    void Move()
    {
        if (moveDirection != Vector2.zero)
            rb.velocity = moveDirection * moveSpeed;
        else
            rb.velocity = Vector2.zero;
    }

    void HandleAnimations()
    {
        anim.SetFloat("Speed", rb.velocity.magnitude);

        if(lastHorizontalDir.x < 0)
            sr.flipX = true;
        else if(lastHorizontalDir.x > 0)
            sr.flipX = false;
    }
    public void PickUpBall(Ball ball)
    {
        if (heldBall != null) return;

        heldBall = ball;

        if (!ball.photonView.IsMine)
            ball.photonView.RequestOwnership();

        ball.PickUp(photonView.ViewID);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine) return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            if (ball != null && heldBall == null)
                PickUpBall(ball);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(sr.flipX);
            stream.SendNext(anim.GetFloat("Speed"));
        }
        else 
        {
            sr.flipX = (bool)stream.ReceiveNext();
            anim.SetFloat("Speed", (float)stream.ReceiveNext());
        }
    }
}
