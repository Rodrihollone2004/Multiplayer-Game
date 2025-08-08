using Photon.Pun;
using TMPro;
using UnityEngine;

public class Username : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject usernamePage;
    public TMP_Text myUsername;

    void Start()
    {
        if (PlayerPrefs.GetString("Username") == "" || PlayerPrefs.GetString("Username") == null)
        {
            usernamePage.SetActive(true);
        }
    }

    public void SaveUsername()
    {
        PhotonNetwork.NickName = inputField.text;
        PlayerPrefs.SetString("Username", inputField.text);
        myUsername.text = inputField.text;
        usernamePage.SetActive(false);
    }
}
