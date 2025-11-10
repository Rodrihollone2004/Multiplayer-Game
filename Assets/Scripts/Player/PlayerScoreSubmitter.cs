using Photon.Pun;
using UnityEngine;

public class PlayerScoreSubmitter : MonoBehaviourPun
{
    [PunRPC]
    void RPC_SubmitMyScore(int score)
    {
        Debug.Log($"[{photonView.Owner.NickName}] Me pidieron subir mi score: {score}");

        LeaderboardService.SubmitScore(score, "globalhighscore", response =>
        {
            if (response)
                Debug.Log("Score subido correctamente desde el cliente ganador.");
            else
                Debug.LogError("Fallo al subir score desde el cliente ganador.");
        });
    }
}
