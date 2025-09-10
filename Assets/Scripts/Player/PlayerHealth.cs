using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;
    public bool IsEliminated { get; private set; } = false;

    private LivesUIManager livesUI;

    void Start()
    {
        currentLives = maxLives;
        livesUI = FindObjectOfType<LivesUIManager>();
    }

    [PunRPC]
    void TakeDamage()
    {
        if (IsEliminated) return;

        currentLives--;

        livesUI.UpdatePlayerLives(photonView.Owner.ActorNumber, currentLives);

        if (currentLives <= 0)
        {
            IsEliminated = true;
            Debug.Log($"{photonView.Owner.NickName} fue eliminado");

            GameManagerQuemado.Instance.CheckRemainingPlayers();

            if (photonView.IsMine)
            {
                EnterSpectatorMode();
            }
        }
    }

    public void GetHit()
    {
        photonView.RPC("TakeDamage", RpcTarget.All);
    }

    private void EnterSpectatorMode()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerBallHandler>().enabled = false;

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Debug.Log("Entraste en modo espectador");
    }
}
