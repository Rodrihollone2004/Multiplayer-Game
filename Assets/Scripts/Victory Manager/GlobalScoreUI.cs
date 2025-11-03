using UnityEngine;
using TMPro;
using System.Linq;

public class GlobalScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Update()
    {
        if (GlobalGameManager.Instance == null) return;

        var playerPoints = GlobalGameManager.Instance.GetPlayerPoints();

        var sorted = playerPoints.OrderByDescending(x => x.Value);

        string text = "";
        foreach (var kvp in sorted)
        {
            text += $"{kvp.Key}: {kvp.Value}\n";
        }

        scoreText.text = text;
    }
}
