using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static LeaderboardSenderScript;

public class MenuHandlerScript : MonoBehaviour
{
    public Text highscoreText;
    public Text cannabisStash;
    public Text usernameText;
    public LeaderboardGetterScript leaderboardGetterScript; // Reference to the script that fetches scores
    public Transform scoreListContainer;
    public GameObject scoreEntryPrefab; // Prefab for displaying each score entry
    public void StartGame()
    {
        // Logic to start the game
        SceneManager.LoadScene("GameScene"); // Replace "GameScene" with the actual name of your game scene
        Debug.Log("Game Started");
    }

    public void LoadItemShop()
    {
        Debug.Log("Loading Item Shop Scene");
        SceneManager.LoadScene("ItemShop"); // Load the Item Shop scene
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true; // Ensure the cursor is visible
        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        highscoreText.text = highscore.ToString();
        Debug.Log("Highscore loaded: " + highscore);
        string username = PlayerPrefs.GetString("Username", "Guest");
        usernameText.text = username.ToString();
        Debug.Log("Starte GetScores Coroutine");
        StartCoroutine(leaderboardGetterScript.GetScores(ShowScores));
        cannabisStash.text = PlayerPrefs.GetInt("CannabisStash", 0).ToString();
        Debug.Log("Cannabis stash loaded: " + cannabisStash.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
            Destroy(child.gameObject); // vorherige Eintr�ge entfernen
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
}
