using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform holdPoint;

    [Space(20)]
    [SerializeField] float dashForce = 15f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 5f;
    bool canDash = true;
    bool isDashing = false;

    [Header("Visuals")]
    public GameObject mark;
    Animator anim;
    SpriteRenderer sr;

    [Header("UI Dash Cooldown")]
    [SerializeField] private Image dashCooldownFill;
    float dashTimer = 0f;

    [Header("Charged Throw")]
    [SerializeField] private float minThrowSpeed = 12f;
    [SerializeField] private float maxThrowSpeed = 25f;
    [SerializeField] private float chargeTime = 1.5f;
    [SerializeField] private Image chargeMeterFill;

    private bool isCharging = false;
    private float currentCharge = 0f;

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
            if (!isCharging) 
            {
                ProcessInputs();
                HandleAnimations();

                if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
                    StartCoroutine(Dash());

                UpdateDashUI();
            }
        }

        UpdateHoldPointPosition();

        if (heldBall != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isCharging = true;
                currentCharge = 0f;
                rb.velocity = Vector2.zero;
            }

            if (Input.GetKey(KeyCode.Space) && isCharging)
            {
                currentCharge += Time.deltaTime;
                currentCharge = Mathf.Clamp(currentCharge, 0f, chargeTime);

                if (chargeMeterFill != null)
                    chargeMeterFill.fillAmount = currentCharge / chargeTime;
            }

            if (Input.GetKeyUp(KeyCode.Space) && isCharging)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = (mousePos - (Vector2)transform.position).normalized;

                float throwSpeed = currentCharge > 0f ? Mathf.Lerp(minThrowSpeed, maxThrowSpeed, currentCharge / chargeTime) : minThrowSpeed;

                heldBall.Throw(dir, photonView.ViewID, throwSpeed);
                heldBall = null;

                isCharging = false;

                if (chargeMeterFill != null)
                    chargeMeterFill.fillAmount = 0f;
            }
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
        if (isDashing || isCharging) return;

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

    void UpdateHoldPointPosition()
    {
        if (sr.flipX)
            holdPoint.localPosition = new Vector3(-Mathf.Abs(holdPoint.localPosition.x), holdPoint.localPosition.y, holdPoint.localPosition.z);
        else
            holdPoint.localPosition = new Vector3(Mathf.Abs(holdPoint.localPosition.x), holdPoint.localPosition.y, holdPoint.localPosition.z);
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

    void UpdateDashUI()
    {
        if (dashCooldownFill == null) return;

        if (!canDash)
        {
            dashTimer -= Time.deltaTime;
            dashCooldownFill.fillAmount = 1f - (dashTimer / dashCooldown);
        }
        else
        {
            dashCooldownFill.fillAmount = 1f;
        }
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

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}
