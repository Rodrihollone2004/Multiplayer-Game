using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerBallHandler : MonoBehaviourPunCallbacks
{
    [Header("Ball Handling")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float minThrowSpeed = 12f;
    [SerializeField] private float maxThrowSpeed = 25f;
    [SerializeField] private float chargeTime = 1.5f;

    private Ball heldBall = null;
    private bool isCharging = false;
    private float currentCharge = 0f;

    private SpriteRenderer sr;

    public bool IsCharging => isCharging;
    public float ChargeProgress => currentCharge / chargeTime;
    public Transform HoldPoint => holdPoint;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateHoldPointPosition();
        if (photonView.IsMine)
        {
            HandleBallInput();
        }
    }

    void UpdateHoldPointPosition()
    {
        if (sr.flipX)
            holdPoint.localPosition = new Vector3(-Mathf.Abs(holdPoint.localPosition.x), holdPoint.localPosition.y, holdPoint.localPosition.z);
        else
            holdPoint.localPosition = new Vector3(Mathf.Abs(holdPoint.localPosition.x), holdPoint.localPosition.y, holdPoint.localPosition.z);
    }

    void HandleBallInput()
    {
        if (heldBall != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isCharging = true;
                currentCharge = 0f;
            }

            if (Input.GetKey(KeyCode.Space) && isCharging)
            {
                currentCharge += Time.deltaTime;
                currentCharge = Mathf.Clamp(currentCharge, 0f, chargeTime);
            }

            if (Input.GetKeyUp(KeyCode.Space) && isCharging)
            {
                ThrowBall();
                isCharging = false;
            }
        }
    }

    void ThrowBall()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;

        float throwSpeed = currentCharge > 0f ?
            Mathf.Lerp(minThrowSpeed, maxThrowSpeed, currentCharge / chargeTime) : minThrowSpeed;

        heldBall.Throw(dir, photonView.ViewID, throwSpeed);
        heldBall = null;
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

            if (ball != null && heldBall == null && ball.CanBePickedUp())
            {
                PickUpBall(ball);
            }
        }
    }

    public bool CanPickUpBall(Ball ball)
    {
        return heldBall == null && !ball.IsHeld;
    }

}