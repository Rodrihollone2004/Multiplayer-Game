using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rb;
    Vector2 moveDirection;
    public GameObject mark;

    Animator anim;
    SpriteRenderer sr;

    Vector2 lastHorizontalDir = Vector2.right;

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
