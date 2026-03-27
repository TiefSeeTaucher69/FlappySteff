using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;
using static LeaderboardSenderScript;

public class MenuHandlerScript : MonoBehaviour
{
    public Text highscoreText;
    public Text cannabisStash;
    public Text usernameText;
    public LeaderboardGetterScript leaderboardGetterScript; // Reference to the script that fetches scores
    public Transform scoreListContainer;
    public GameObject scoreEntryPrefab; // Prefab for displaying each score entry
    public WeeklyMissionUI weeklyMissionUI; // Reference to the WeeklyMissionUI script
    public WeeklyMissionRewardScript weeklyMissionRewardScript; // Inspector zuweisen

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
            SceneManager.LoadScene("EscapeScene");
        }
    }

    public void ShowScores(List<ScoreData> scores)
    {
        Debug.Log(scores == null ? "ShowScores: scores ist null" : $"ShowScores: {scores.Count} Eintr�ge");
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
            Text[] texts = entryGO.GetComponentsInChildren<Text>();
            if (texts.Length >= 2)
            {
                texts[0].text = entry.username;
                texts[1].text = entry.score.ToString();
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
