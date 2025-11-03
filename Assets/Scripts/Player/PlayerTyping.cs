using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerTyping : MonoBehaviourPunCallbacks
{
    private TMP_Text inputDisplay;
    private string currentInput = "";
    private StepMover stepMover;
    private bool canType = false;

    void Start()
    {
        stepMover = GetComponent<StepMover>();

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        string textName = $"PlayerInputText_{actorNumber}";
        GameObject go = GameObject.Find(textName);

        if (go != null)
            inputDisplay = go.GetComponent<TMP_Text>();
        else
            Debug.LogWarning($"⚠ No se encontró {textName} en la escena");

        canType = photonView.IsMine;
    }

    public void SetCanType(bool value)
    {
        canType = value;
    }

    void Update()
    {
        if (!canType) return;
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

        UpdateWordColors();

        if (currentInput.Equals(WordManager.Instance.currentWord, System.StringComparison.OrdinalIgnoreCase))
        {
            photonView.RPC("RPC_WinWord", RpcTarget.All, photonView.ViewID);
        }
    }

    [PunRPC]
    void RPC_WinWord(int winnerViewID)
    {
        PhotonView winnerView = PhotonView.Find(winnerViewID);
        if (winnerView == null) return;

        StepMover mover = winnerView.GetComponent<StepMover>();
        if (mover != null)
            mover.MoveUpOneStep();

        if (PhotonNetwork.IsMasterClient)
            WordManager.Instance.PlayerCompleted(winnerView.Owner.NickName);

        if (winnerView.IsMine)
        {
            currentInput = "";
            if (inputDisplay != null)
                inputDisplay.text = "";
        }
    }

    void UpdateWordColors()
    {
        if (WordManager.Instance == null || string.IsNullOrEmpty(WordManager.Instance.currentWord))
            return;

        string targetWord = WordManager.Instance.currentWord;
        string colored = "";

        for (int i = 0; i < currentInput.Length; i++)
        {
            char inputChar = currentInput[i];
            char targetChar = targetWord[i];

            if (char.ToLower(inputChar) == char.ToLower(targetChar))
                colored += $"<color=yellow>{inputChar}</color>";
            else
                colored += $"<color=red>{inputChar}</color>";
        }

        if (inputDisplay != null)
            inputDisplay.text = colored;
    }
}
