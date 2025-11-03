using Photon.Pun;
using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviourPunCallbacks
{
    public static WordManager Instance;

    [SerializeField] TMP_Text currentWordText;
    [SerializeField] string[] words;
    public string currentWord { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (currentWordText == null)
            currentWordText = GameObject.Find("WordText").GetComponent<TMP_Text>();

        if (PhotonNetwork.IsMasterClient)
            GenerateNewWord();
    }

    [PunRPC]
    void RPC_SetWord(string word)
    {
        currentWord = word;
        currentWordText.text = word;
    }

    public void GenerateNewWord()
    {
        string newWord = words[Random.Range(0, words.Length)];
        photonView.RPC("RPC_SetWord", RpcTarget.All, newWord);
    }

    [PunRPC]
    void RPC_PlayerCompleted(string playerName)
    {
        Debug.Log($"{playerName} completó la palabra primero!");
        if (PhotonNetwork.IsMasterClient)
            GenerateNewWord();
    }

    public void PlayerCompleted(string playerName)
    {
        photonView.RPC("RPC_PlayerCompleted", RpcTarget.All, playerName);
    }
}
