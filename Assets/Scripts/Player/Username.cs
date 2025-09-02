using Photon.Pun;
using TMPro;
using UnityEngine;

public class Username : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject usernamePage;
    public TMP_Text myUsername;

    [Header("Error UI")]
    [SerializeField] private TMP_Text errorMessage;
    [SerializeField] private GameObject errorPanel;

    void Start()
    {
        if (PlayerPrefs.GetString("Username") == "" || PlayerPrefs.GetString("Username") == null)
        {
            usernamePage.SetActive(true);
        }
    }

    public void SaveUsername()
    {
        string enteredName = inputField.text.Trim();

        if (string.IsNullOrEmpty(enteredName))
        {
            if (errorMessage != null)
                errorMessage.text = "Your name cannot be empty.";

            if (errorPanel != null)
                errorPanel.SetActive(true);

            return;
        }

        PhotonNetwork.NickName = inputField.text;
        PlayerPrefs.SetString("Username", inputField.text);
        myUsername.text = inputField.text.ToUpper();
        usernamePage.SetActive(false);
    }
}
