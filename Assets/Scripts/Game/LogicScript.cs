using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public int highScore;
    public Text scoreText;
    public Text cannabisStashText;
    public GameObject gameOverScreen;
    public GameObject menuScreen;
    public LeaderboardSenderScript leaderboardSenderScript;
    private bool hasGameOverBeenHandled = false;
    [SerializeField] private GameObject cannabisCollectedPrefab;
    [SerializeField] private Transform playerHead; // Position über dem Charakter

    [ContextMenu("Increase CannabisScore")]
    public void addCannabisScore(int scoreToAdd)
    {
        Debug.Log("Adding cannabis score: " + scoreToAdd);
        PlayerPrefs.SetInt("CannabisStash", PlayerPrefs.GetInt("CannabisStash", 0) + scoreToAdd);
        PlayerPrefs.Save();
        cannabisStashText.text = PlayerPrefs.GetInt("CannabisStash", 0).ToString();

        // Animation starten
        if (cannabisCollectedPrefab != null && playerHead != null)
        {
            Debug.Log("Instantiating Cannabis Animation Icon");
            GameObject icon = Instantiate(cannabisCollectedPrefab, playerHead.position + Vector3.up * 1f, Quaternion.identity, playerHead);

            AudioSource audio = icon.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }

            StartCoroutine(AnimateCannabisCollected(icon.transform));
        }
    }

    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        playerScore = playerScore + scoreToAdd;
        scoreText.text = playerScore.ToString();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        if (hasGameOverBeenHandled) return;
        hasGameOverBeenHandled = true;

        gameOverScreen.SetActive(true);
        Cursor.visible = true;  
        if (playerScore > highScore)
        {
            PlayerPrefs.SetInt("Highscore", playerScore);
            PlayerPrefs.Save();
            Debug.Log("New high score saved: " + playerScore);
            string username = PlayerPrefs.GetString("Username", "Anonymous");
            StartCoroutine(leaderboardSenderScript.SendScore(username, playerScore));
        }
        else
        {
            Debug.Log("Kein neuer Highscore. Aktueller: " + highScore);
        }

        SpeedManager.ResetSpeed();
        SpeedManagerCannabisScript.ResetSpeed();
    }

    public void backtoMenu()
    {
        Debug.Log("Going to main menu");
        SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("Highscore", 0);
        leaderboardSenderScript = GameObject.Find("LeaderboardSender").GetComponent<LeaderboardSenderScript>();
        cannabisStashText.text = PlayerPrefs.GetInt("CannabisStash", 0).ToString();
    }

    private IEnumerator AnimateCannabisCollected(Transform iconTransform)
    {
        Vector3 startPos = iconTransform.localPosition;
        Quaternion startRot = iconTransform.rotation;

        float jumpHeight = 1f;
        float rotationAmount = 360f;
        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float height = 4 * jumpHeight * t * (1 - t);
            iconTransform.localPosition = startPos + Vector3.up * height;
            iconTransform.rotation = startRot * Quaternion.Euler(0, rotationAmount * t, 0);

            yield return null;
        }

        Destroy(iconTransform.gameObject); // Entferne nach Animation
    }

}
