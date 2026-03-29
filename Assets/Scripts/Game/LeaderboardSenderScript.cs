using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using UnityEngine;

public class LeaderboardSenderScript : MonoBehaviour
{
    public const string LeaderboardId = "FlappySteffLeaderboard";

    public async Task SendScore(int score)
    {
        try
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
            Debug.Log("Score erfolgreich gesendet: " + score);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Fehler beim Senden des Scores: " + e.Message);
        }
    }

    [System.Serializable]
    public class ScoreData
    {
        public string username;
        public int score;
        public int rank;
    }
}
