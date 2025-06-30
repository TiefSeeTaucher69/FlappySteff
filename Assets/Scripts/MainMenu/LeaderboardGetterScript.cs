using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static LeaderboardSenderScript;

public class LeaderboardGetterScript : MonoBehaviour
{
    [Header("UI")]
    public GameObject errorPanel;  // Das UI Panel für Fehler

    public IEnumerator GetScores(System.Action<List<ScoreData>> callback)
    {
        string url = "https://api.benjo.online/scores";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Fehler beim Laden des Scoreboards: " + request.error);

            // Fehler-Panel anzeigen, falls zugewiesen
            if (errorPanel != null)
                errorPanel.SetActive(true);

            callback(null);
        }
        else
        {
            // Fehler-Panel ausblenden, falls sichtbar
            if (errorPanel != null && errorPanel.activeSelf)
                errorPanel.SetActive(false);

            string json = request.downloadHandler.text;
            Debug.Log("Scoreboard JSON: " + json);
            ScoreList list = JsonUtility.FromJson<ScoreList>("{\"scores\":" + json + "}");
            Debug.Log("ScoreList count: " + (list.scores != null ? list.scores.Count : 0));
            callback(list.scores);
        }
    }

    [System.Serializable]
    public class ScoreList
    {
        public List<ScoreData> scores;
    }
}
