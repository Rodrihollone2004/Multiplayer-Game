using TMPro;
using UnityEngine;

public class ErrorUI : MonoBehaviour
{
    public static ErrorUI Instance { get; private set; }

    [Header("Error UI")]
    [SerializeField] GameObject errorPanel;
    [SerializeField] TMP_Text errorMessageText;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowError(string message)
    {
        errorMessageText.text = message;
        errorPanel.SetActive(true);
    }

    public void HideError()
    {
        errorPanel.SetActive(false);
    }
}
