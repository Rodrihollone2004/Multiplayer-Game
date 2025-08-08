using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rb;
    Vector2 moveDirection;
    public GameObject mark;

    void Update()
    {
        if (photonView.IsMine)
            ProcessInputs();
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
    }
    void Move()
    {
        if (moveDirection != Vector2.zero)
            rb.velocity = moveDirection * moveSpeed;
        else
            rb.velocity = Vector2.zero;
    }
}
