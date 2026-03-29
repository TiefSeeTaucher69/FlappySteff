using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;
using static LeaderboardSenderScript;

public class MenuHandlerScript : MonoBehaviour
{
    public TMPro.TMP_Text highscoreText;
    public TMPro.TMP_Text cannabisStash;
    public TMPro.TMP_Text usernameText;
    public LeaderboardGetterScript leaderboardGetterScript; // Reference to the script that fetches scores
    public Transform scoreListContainer;
    public GameObject scoreEntryPrefab; // Prefab for displaying each score entry
    public WeeklyMissionUI weeklyMissionUI; // Reference to the WeeklyMissionUI script
    public WeeklyMissionRewardScript weeklyMissionRewardScript; // Inspector zuweisen
    public GameObject quitPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("Game Started");
    }

    public void LoadItemShop()
    {
        Debug.Log("Loading Item Shop Scene");
        SceneManager.LoadScene("ItemShop");
    }

    async void Start()
    {
        Cursor.visible = true;

        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        highscoreText.text = highscore.ToString();
        Debug.Log("Highscore loaded: " + highscore);

        string username = PlayerPrefs.GetString("Username", "Guest");
        usernameText.text = username.ToString();

        Debug.Log("Starte GetScores");
        _ = leaderboardGetterScript.GetScores(ShowScores);

        cannabisStash.text = PlayerPrefs.GetInt("CannabisStash", 0).ToString();
        Debug.Log("Cannabis stash loaded: " + cannabisStash.text);

        Debug.Log("WeeklyMissionRewardScript im MenuHandler: " + (weeklyMissionRewardScript != null));
        var missionManager = WeeklyMissionManager.Instance;
        if (missionManager != null)
        {
            missionManager.OnMissionsLoaded += OnMissionsLoaded;
            missionManager.weeklyMissionRewardScript = weeklyMissionRewardScript;
            // Jetzt Missionen neu laden und UI danach updaten
            missionManager.ReloadMissions();
        }
        else
        {
            Debug.LogWarning("WeeklyMissionManager.Instance ist null im MenuHandler");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (quitPanel != null) quitPanel.SetActive(true);
        }
    }

    public void QuitGame() => Application.Quit();
    public void CloseQuitPanel() { if (quitPanel != null) quitPanel.SetActive(false); }

    public void ShowScores(List<ScoreData> scores)
    {
        Debug.Log(scores == null ? "ShowScores: scores ist null" : $"ShowScores: {scores.Count} Eintr\u00e4ge");
        if (scores == null)
        {
            Debug.LogError("ShowScores wurde mit null aufgerufen.");
            return;
        }

        foreach (Transform child in scoreListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in scores)
        {
            if (entry == null) continue;

            GameObject entryGO = Instantiate(scoreEntryPrefab, scoreListContainer);
            Debug.Log(entryGO.name);
            TMPro.TMP_Text[] texts = entryGO.GetComponentsInChildren<TMPro.TMP_Text>();
            if (texts.Length >= 3)
            {
                texts[0].text = "#" + entry.rank;
                texts[1].text = entry.username;
                texts[2].text = entry.score.ToString();

                Color entryColor = entry.rank switch
                {
                    1 => new Color(1.00f, 0.84f, 0.00f), // Gold
                    2 => new Color(0.75f, 0.75f, 0.75f), // Silber
                    3 => new Color(0.80f, 0.50f, 0.20f), // Bronze
                    _ => Color.white
                };
                foreach (var t in texts) t.color = entryColor;
            }
        }
    }

    private void OnMissionsLoaded()
    {
        Debug.Log("Missions wurden geladen - UpdateUI wird aufgerufen");
        if (weeklyMissionUI != null)
        {
            weeklyMissionUI.UpdateUI();
        }
    }

    private void OnDestroy()
    {
        var missionManager = WeeklyMissionManager.Instance;
        if (missionManager != null)
        {
            missionManager.OnMissionsLoaded -= OnMissionsLoaded;
        }
    }
}
