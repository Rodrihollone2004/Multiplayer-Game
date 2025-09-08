using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    [SerializeField] private Image dashCooldownFill;
    [SerializeField] private Image chargeMeterFill;

    private PlayerMovement playerMovement;
    private PlayerBallHandler playerBallHandler;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerBallHandler = GetComponent<PlayerBallHandler>();

        if (photonView.IsMine)
        {
            if (dashCooldownFill != null)
                dashCooldownFill.fillAmount = 1f;

            if (chargeMeterFill != null)
                chargeMeterFill.fillAmount = 0f;
        }
        else
        {
            if (dashCooldownFill != null)
                dashCooldownFill.gameObject.SetActive(false);

            if (chargeMeterFill != null)
                chargeMeterFill.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        UpdateDashUI();
        UpdateChargeUI();
    }

    private void UpdateDashUI()
    {
        if (dashCooldownFill == null || playerMovement == null) return;

        dashCooldownFill.fillAmount = playerMovement.DashCooldownProgress;

        dashCooldownFill.color = playerMovement.CanDash ? Color.green : Color.gray;
    }

    private void UpdateChargeUI()
    {
        if (chargeMeterFill == null || playerBallHandler == null) return;

        if (playerBallHandler.IsCharging)
        {
            chargeMeterFill.fillAmount = playerBallHandler.ChargeProgress;
            chargeMeterFill.color = Color.Lerp(Color.yellow, Color.red, playerBallHandler.ChargeProgress);
        }
        else
        {
            chargeMeterFill.fillAmount = 0f;
        }
    }
}