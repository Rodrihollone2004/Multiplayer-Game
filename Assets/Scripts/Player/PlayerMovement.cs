using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float dashForce = 15f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 5f;
    [SerializeField] PlayerBallHandler playerBallHandler;

    public GameObject mark;

    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimer = 0f;

    public bool CanDash => canDash;
    public float DashCooldownProgress => 1f - Mathf.Clamp01(dashTimer / dashCooldown);

    private Vector2 moveDirection;
    private Vector2 lastHorizontalDir = Vector2.right;

    private Animator anim;
    private SpriteRenderer sr;

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

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
                StartCoroutine(Dash());
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

        if (moveX != 0)
            lastHorizontalDir = new Vector2(moveX, 0);
    }

    void Move()
    {
        if (isDashing) return;

        if (playerBallHandler != null && playerBallHandler.IsCharging)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (moveDirection != Vector2.zero)
            rb.velocity = moveDirection * moveSpeed;
        else
            rb.velocity = Vector2.zero;
    }

    void HandleAnimations()
    {
        anim.SetFloat("Speed", rb.velocity.magnitude);

        if (lastHorizontalDir.x < 0)
            sr.flipX = true;
        else if (lastHorizontalDir.x > 0)
            sr.flipX = false;
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDir = (mousePos - (Vector2)transform.position).normalized;

        rb.velocity = dashDir * dashForce;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        dashTimer = dashCooldown;
        isDashing = false;

        float elapsed = 0f;
        while (elapsed < dashCooldown)
        {
            dashTimer = dashCooldown - elapsed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        canDash = true;
        dashTimer = 0f;
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