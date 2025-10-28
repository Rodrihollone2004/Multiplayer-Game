using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerTyping : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text inputDisplay;
    private string currentInput = "";
    private StepMover stepMover;

    void Start()
    {
        stepMover = GetComponent<StepMover>();

        if (inputDisplay == null)
        {
            GameObject go = GameObject.Find("PlayerInputText");
            if (go != null)
                inputDisplay = go.GetComponent<TMP_Text>();
        }

        if (!photonView.IsMine)
        {
            if (inputDisplay != null)
                inputDisplay.gameObject.SetActive(false);
            enabled = false;
        }
    }

    void Update()
    {
        if (WordManager.Instance == null || string.IsNullOrEmpty(WordManager.Instance.currentWord))
            return;

        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else if (char.IsLetter(c))
            {
                if (currentInput.Length < WordManager.Instance.currentWord.Length)
                    currentInput += c;
            }
        }

        if (inputDisplay != null)
            inputDisplay.text = currentInput;

        if (currentInput.Equals(WordManager.Instance.currentWord, System.StringComparison.OrdinalIgnoreCase))
        {
            photonView.RPC("RPC_WinWord", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_WinWord()
    {
        if (!photonView.IsMine) return;

        stepMover.MoveUpOneStep();
        WordManager.Instance.PlayerCompleted(PhotonNetwork.NickName);
        currentInput = "";

        if (inputDisplay != null)
            inputDisplay.text = "";
    }
}
