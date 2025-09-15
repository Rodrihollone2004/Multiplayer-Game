using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    [SerializeField] private Image dashCooldownFill;
    [SerializeField] private Image chargeMeterFill;
    [SerializeField] private Image chargeParryFill;

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

            if (chargeParryFill != null)
                chargeParryFill.fillAmount = 1f;
        }
        else
        {
            if (dashCooldownFill != null)
                dashCooldownFill.gameObject.SetActive(false);

            if (chargeMeterFill != null)
                chargeMeterFill.gameObject.SetActive(false);

            if(chargeParryFill != null)
                chargeParryFill.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        UpdateParryUI();
        UpdateDashUI();
        UpdateChargeUI();
    }

    private void UpdateParryUI()
    {
        if (chargeParryFill == null || playerBallHandler == null) return;

        chargeParryFill.fillAmount = playerBallHandler.ParryCooldownProgress;

        chargeParryFill.color = playerBallHandler.IsParryReady ? Color.green : Color.gray;
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
            chargeMeterFill.color = Color.Lerp(Color.green, Color.red, playerBallHandler.ChargeProgress);
        }
        else
        {
            chargeMeterFill.fillAmount = 0f;
        }
    }
}