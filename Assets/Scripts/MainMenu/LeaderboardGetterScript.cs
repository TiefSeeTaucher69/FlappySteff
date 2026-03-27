using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using UnityEngine;
using static LeaderboardSenderScript;

public class LeaderboardGetterScript : MonoBehaviour
{
    [Header("UI")]
    public GameObject errorPanel;

    public async Task GetScores(System.Action<List<ScoreData>> callback)
    {
        try
        {
            var response = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardSenderScript.LeaderboardId);

            if (errorPanel != null && errorPanel.activeSelf)
                errorPanel.SetActive(false);

            var scores = new List<ScoreData>();
            foreach (var entry in response.Results)
            {
                scores.Add(new ScoreData
                {
                    username = entry.PlayerName,
                    score = (int)entry.Score
                });
            }
            callback(scores);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Fehler beim Laden des Scoreboards: " + e.Message);
            if (errorPanel != null)
                errorPanel.SetActive(true);
            callback(null);
        }
    }
}
