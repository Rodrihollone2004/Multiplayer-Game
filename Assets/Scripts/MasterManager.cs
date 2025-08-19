using Photon.Pun;
using UnityEngine;
using TMPro;

public class MasterManager : MonoBehaviour
{
    public static MasterManager Instance;

    [Header("UI")]
    [SerializeField] GameObject configPanel;
    [SerializeField] TMP_Text hintText;

    private bool canOpenPanel = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && canOpenPanel)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                configPanel.SetActive(!configPanel.activeSelf);
            }
        }
    }

    public void SetCanOpenPanel(bool value)
    {
        canOpenPanel = value;

        if (PhotonNetwork.IsMasterClient && hintText != null)
            hintText.gameObject.SetActive(value);

        if (!value)
            configPanel.SetActive(false);
    }
}
