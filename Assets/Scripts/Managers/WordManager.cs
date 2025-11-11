using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WordManager : MonoBehaviourPunCallbacks
{
    public static WordManager Instance;

    [SerializeField] TMP_Text currentWordText;
    [SerializeField] List<string> words;
    List<string> shuffledList;
    public string currentWord { get; private set; }
    private List<string> usedWords = new List<string>();
    string newWord = null;
    int index;

    void Awake()
    {
        Instance = this;
        index = 0;
    }

    void Start()
    {
        if (currentWordText == null)
            currentWordText = GameObject.Find("WordText").GetComponent<TMP_Text>();

        if (photonView.IsMine)
        {
            shuffledList = words.OrderBy(x => Random.value).ToList();
            string serialized = string.Join("|", shuffledList);
            photonView.RPC("RPC_SetupWords", RpcTarget.All, serialized);
        }
    }

    [PunRPC]
    void RPC_SetupWords(string serialized)
    {
        shuffledList = serialized.Split('|').ToList();
        index = 0;
        SetWord(index);
    }

    public void GenerateNewWord()
    {
        if (usedWords.Count >= shuffledList.Count)
        {
            usedWords.Clear();
            index = 0;
        }
        else
            index++;

        photonView.RPC("RPC_SetWord", RpcTarget.All, index);
    }

    [PunRPC]
    void RPC_SetWord(int newIndex)
    {
        index = newIndex;
        SetWord(index);
    }

    void SetWord(int i)
    {
        if (i >= shuffledList.Count)
            return;

        currentWord = shuffledList[i];
        currentWordText.text = currentWord;
        usedWords.Add(currentWord);
    }

    [PunRPC]
    void RPC_PlayerCompleted(string playerName)
    {
        Debug.Log($"{playerName} completó la palabra primero!");
        if (photonView.IsMine)
            GenerateNewWord();
    }

    public void PlayerCompleted(string playerName)
    {
        photonView.RPC("RPC_PlayerCompleted", RpcTarget.All, playerName);
    }
}
