using Photon.Pun;
using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text currentWordText;
    [SerializeField] string[] words;
    private string currentWord;
    private string currentInput = "";
    private StepMover stepMover;

    void Start()
    {
        stepMover = GetComponent<StepMover>();

        if (currentWordText == null)
            currentWordText = GameObject.Find("WordText").GetComponent<TMP_Text>();

        if (!photonView.IsMine)
        {
            currentWordText.gameObject.SetActive(false);
            enabled = false;
            return;
        }

        GenerateNewWord();
    }

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else if (char.IsLetter(c))
            {
                if (currentInput.Length < currentWord.Length)
                    currentInput += c;
            }
        }

        UpdateWordColors();

        if (currentInput.Equals(currentWord, System.StringComparison.OrdinalIgnoreCase))
        {
            stepMover.MoveUpOneStep();
            GenerateNewWord();
        }
    }

    void GenerateNewWord()
    {
        currentWord = words[Random.Range(0, words.Length)];
        currentInput = "";
        currentWordText.text = currentWord;
    }

    void UpdateWordColors()
    {
        string colored = "";

        for (int i = 0; i < currentWord.Length; i++)
        {
            if (i < currentInput.Length)
            {
                if (char.ToLower(currentInput[i]) == char.ToLower(currentWord[i]))
                    colored += $"<color=yellow>{currentWord[i]}</color>";
                else
                    colored += $"<color=red>{currentWord[i]}</color>";
            }
            else
            {
                colored += $"<color=white>{currentWord[i]}</color>";
            }
        }

        currentWordText.text = colored;
    }
}
