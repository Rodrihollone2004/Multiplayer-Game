using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerTyping : MonoBehaviourPunCallbacks
{
    private TMP_Text inputDisplay;
    private string currentInput = "";
    private string lastSyncedInput = "";
    private StepMover stepMover;
    private bool canType = false;
    private float syncInterval = 0.1f; // enviar RPC como maximo cada 0.1s
    private float syncTimer = 0f;

    void Start()
    {
        stepMover = GetComponent<StepMover>();

        int actorNumber = photonView.Owner.ActorNumber;
        string textName = $"PlayerInputText_{actorNumber}";
        GameObject go = GameObject.Find(textName);

        if (go != null)
            inputDisplay = go.GetComponent<TMP_Text>();

        canType = photonView.IsMine;
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

        UpdateWordColors(currentInput);

        syncTimer += Time.deltaTime;
        if (syncTimer >= syncInterval && currentInput != lastSyncedInput)
        {
            photonView.RPC("RPC_UpdateInputDisplay", RpcTarget.Others, currentInput);
            lastSyncedInput = currentInput;
            syncTimer = 0f;
        }

        if (currentInput.Equals(WordManager.Instance.currentWord, System.StringComparison.OrdinalIgnoreCase))
        {
            photonView.RPC("RPC_WinWord", RpcTarget.All, photonView.ViewID);
        }
    }

    [PunRPC]
    void RPC_UpdateInputDisplay(string input)
    {
        UpdateWordColors(input);
    }

    void UpdateWordColors(string input)
    {
        if (inputDisplay == null || WordManager.Instance == null) return;

        string targetWord = WordManager.Instance.currentWord;
        string colored = "";

        for (int i = 0; i < input.Length && i < targetWord.Length; i++)
        {
            char inputChar = input[i];
            char targetChar = targetWord[i];

            if (char.ToLower(inputChar) == char.ToLower(targetChar))
                colored += $"<color=white>{inputChar}</color>";
            else
                colored += $"<color=red>{inputChar}</color>";
        }

        inputDisplay.text = colored;
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

        foreach (var typing in FindObjectsOfType<PlayerTyping>())
        {
            typing.ClearInputLocal();
        }
    }

    public void ClearInputLocal()
    {
        currentInput = "";
        lastSyncedInput = "";
        if (inputDisplay != null)
            inputDisplay.text = "";
    }
}
