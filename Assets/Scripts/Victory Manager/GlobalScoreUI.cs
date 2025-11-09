using UnityEngine;
using TMPro;
using System.Linq;

public class GlobalScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        Invoke(nameof(ShowLeaderboard), 1f);
    }

    private void ShowLeaderboard()
    {
        LeaderboardUI leaderboard = FindObjectOfType<LeaderboardUI>();

        if (leaderboard != null)
        {
            leaderboard.gameObject.SetActive(true);
            leaderboard.Refresh();
        }
        else
        {
            Debug.LogWarning("No se encontró el LeaderboardUI en el Lobby.");
        }
    }

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
