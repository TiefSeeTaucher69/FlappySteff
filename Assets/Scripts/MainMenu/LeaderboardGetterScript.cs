using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using UnityEngine;
using static LeaderboardSenderScript;

public class LeaderboardGetterScript : MonoBehaviour
{
    [Header("UI")]
    public GameObject errorPanel;

    public async Task<List<ScoreData>> GetScores()
    {
        try
        {
            Debug.Log("[LB] GetScores gestartet: " + LeaderboardSenderScript.LeaderboardId);
            var response = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardSenderScript.LeaderboardId);

            if (errorPanel != null && errorPanel.activeSelf)
                errorPanel.SetActive(false);

            var scores = new List<ScoreData>();
            Debug.Log($"[LB] GetScores Antwort: {response.Results.Count} Einträge");
            foreach (var entry in response.Results)
            {
                Debug.Log($"  [LB]  Rank={entry.Rank + 1}, Name='{entry.PlayerName}', Score={entry.Score}");
                scores.Add(new ScoreData
                {
                    username = entry.PlayerName,
                    score    = (int)entry.Score,
                    rank     = entry.Rank + 1
                });
            }
            return scores;
        }
        catch (System.Exception e)
        {
            Debug.LogError("[LB] Fehler beim Laden des Scoreboards: " + e.Message);
            if (errorPanel != null)
                errorPanel.SetActive(true);
            return null;
        }
    }

    public async Task<List<ScoreData>> GetRankedScores()
    {
        try
        {
            Debug.Log("[LB] GetRankedScores gestartet: " + LeaderboardSenderScript.RankedLeaderboardId);
            var response = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardSenderScript.RankedLeaderboardId);

            if (errorPanel != null && errorPanel.activeSelf)
                errorPanel.SetActive(false);

            var scores = new List<ScoreData>();
            Debug.Log($"[LB] GetRankedScores Antwort: {response.Results.Count} Einträge");
            foreach (var entry in response.Results)
            {
                Debug.Log($"  [LB]  Rank={entry.Rank + 1}, Name='{entry.PlayerName}', Score={entry.Score}");
                scores.Add(new ScoreData
                {
                    username = entry.PlayerName,
                    score    = (int)entry.Score,
                    rank     = entry.Rank + 1
                });
            }
            return scores;
        }
        catch (System.Exception e)
        {
            Debug.LogError("[LB] Fehler beim Laden des Ranked Scoreboards: " + e.Message);
            if (errorPanel != null)
                errorPanel.SetActive(true);
            return null;
        }
    }
}
