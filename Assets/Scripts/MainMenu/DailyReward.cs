using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;

public class DailyReward : MonoBehaviour
{
    public int cannabisRewardToAdd = 5; // Belohnung für tägliches Einloggen

    [Header("UI Elemente")]
    public GameObject panel;
    public Text statusText;
    public Button rewardButton;
    public Image rewardImage;
    public Sprite rewardSprite;
    public Text cannabisStashText;
    


    [Header("Audio")]
    public AudioSource coinAudioSource;

    private string currentDate;
    private const string rewardKey = "lastRewardDate";

    void Start()
    {
        Debug.Log("🔄 Starte DailyReward...");
        rewardButton.interactable = false;
        rewardButton.onClick.AddListener(OnRewardButtonClicked);
        rewardImage.sprite = rewardSprite;
        panel.SetActive(false);

        StartCoroutine(GetServerDate());
    }

    IEnumerator GetServerDate()
    {
        string url = "https://api.benjo.online/time";
        Debug.Log("🌐 Anfrage an Server: " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("User-Agent", "UnityApp/1.0");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("✅ Serverantwort erhalten:\n" + json);

            WorldClockApiResponse timeResponse = null;
            try
            {
                timeResponse = JsonUtility.FromJson<WorldClockApiResponse>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError("❌ Fehler beim JSON-Parsing: " + ex.Message);
                ShowPanel("❌ Fehler beim Verarbeiten der Zeitdaten", false);
                yield break;
            }

            if (timeResponse != null && !string.IsNullOrEmpty(timeResponse.datetime))
            {
                Debug.Log("🕒 Empfangene Zeit (raw): " + timeResponse.datetime);
                if (DateTime.TryParse(timeResponse.datetime, out DateTime parsedDateTime))
                {
                    // DateTimeKind explizit auf UTC setzen
                    DateTime serverDateTimeUtc = DateTime.SpecifyKind(parsedDateTime, DateTimeKind.Utc);

                    // Zeitzonen-Fallback-Logik
                    TimeZoneInfo berlinZone = null;
                    try
                    {
                        berlinZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
                    }
                    catch (TimeZoneNotFoundException)
                    {
                        try
                        {
                            berlinZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                        }
                        catch (TimeZoneNotFoundException)
                        {
                            Debug.LogWarning("⚠️ Berlin-Zeitzone nicht gefunden, benutze UTC");
                        }
                    }

                    DateTime berlinTime = berlinZone != null
                        ? TimeZoneInfo.ConvertTimeFromUtc(serverDateTimeUtc, berlinZone)
                        : serverDateTimeUtc; // Fallback: UTC nutzen

                    currentDate = berlinTime.ToString("yyyy-MM-dd");
                    Debug.Log("📅 Aktuelles Serverdatum (Berlin oder UTC): " + currentDate);

                    CheckRewardAvailability(berlinTime.Date);
                }
                else
                {
                    Debug.LogError("❌ Fehler beim Parsen von DateTime");
                    ShowPanel("❌ Fehler beim Lesen des Datums", false);
                }
            }
            else
            {
                Debug.LogError("❌ JSON enthält kein gültiges 'datetime'-Feld");
                ShowPanel("❌ Unerwartetes Serverformat", false);
            }
        }
        else
        {
            Debug.LogError("❌ Fehler beim Serverzugriff: " + request.error);
            ShowPanel("❌ Serverzeit konnte nicht geladen werden", false);
        }
    }

    void CheckRewardAvailability(DateTime serverDate)
    {
        string savedDateString = PlayerPrefs.GetString(rewardKey, "2000-01-01");
        Debug.Log("📦 Gespeichertes Datum: " + savedDateString);

        if (DateTime.TryParse(savedDateString, out DateTime savedDate))
        {
            if (savedDate < serverDate)
            {
                Debug.Log("Tägliche Belohnung verfügbar!");
                ShowPanel("Tägliche Belohnung verfügbar!", true);
            }
            else
            {
                Debug.Log("Heute bereits abgeholt.");
                // Panel **nicht** anzeigen, wenn Belohnung heute schon abgeholt wurde
                panel.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Kein gespeichertes Datum vorhanden. Erste Belohnung möglich.");
            ShowPanel("Tägliche Belohnung verfügbar!", true);
        }
    }


    void ShowPanel(string message, bool buttonActive)
    {
        statusText.text = message;
        rewardButton.interactable = buttonActive;
        panel.SetActive(true);
        StartCoroutine(FadeInPanel());
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    void OnRewardButtonClicked()
    {
        Debug.Log("🎉 Belohnung eingesammelt!");
        if (coinAudioSource != null)
            coinAudioSource.Play();

        // 🎁 Animation starten
        StartCoroutine(AnimateRewardImage());

        PlayerPrefs.SetString(rewardKey, currentDate);
        rewardButton.interactable = false;

        Debug.Log("Adding cannabis score: " + cannabisRewardToAdd);
        PlayerPrefs.SetInt("CannabisStash", PlayerPrefs.GetInt("CannabisStash", 0) + cannabisRewardToAdd);
        PlayerPrefs.Save();
        cannabisStashText.text = PlayerPrefs.GetInt("CannabisStash", 0).ToString();

        statusText.text = "✅ Belohnung erfolgreich abgeholt!";
    }

    IEnumerator FadeInPanel()
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
        }

        panel.SetActive(true);

        float duration = 0.5f;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    [Serializable]
    public class WorldClockApiResponse
    {
        public string datetime;
    }

    IEnumerator AnimateRewardImage()
    {
        Transform imgTransform = rewardImage.transform;
        Vector3 startPos = imgTransform.localPosition;
        Quaternion startRot = imgTransform.rotation;

        float jumpHeight = 50f;
        float rotationAmount = 360f;
        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Springen (parabolisch)
            float height = 4 * jumpHeight * t * (1 - t);
            imgTransform.localPosition = startPos + Vector3.up * height;

            // Rotation
            imgTransform.rotation = startRot * Quaternion.Euler(0, rotationAmount * t, 0);

            yield return null;
        }

        // Rücksetzen
        imgTransform.localPosition = startPos;
        imgTransform.rotation = startRot;
    }

}

