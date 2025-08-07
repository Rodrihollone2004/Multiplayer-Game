using UnityEngine;
using Photon.Pun;
using TMPro;


public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] float moveSpeed;
    [SerializeField] Rigidbody2D rb;
    Vector2 moveDirection;
    public GameObject mark;
    public GameObject canvasName;
    public TMP_Text Name;


    void Start()
    {
        if(GetComponent<PhotonView>().IsMine == false)
        {
            canvasName.SetActive(true);
            Name.text = GetComponent<PhotonView>().Controller.NickName;
        }
    }

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
