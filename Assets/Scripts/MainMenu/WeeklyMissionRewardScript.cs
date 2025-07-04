﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeeklyMissionRewardScript : MonoBehaviour
{
    public GameObject panel;
    public Text statusText;
    public Button collectButton;
    public Image rewardImage;
    public AudioSource audioSource;
    public Sprite rewardSprite;
    public Text cannabisStashText;

    public int cannabisReward = 15;

    private string currentMissionId;

    void Awake()
    {
        panel.SetActive(false);
        collectButton.onClick.AddListener(OnCollectClicked);
        rewardImage.sprite = rewardSprite;
    }

    /// <summary>
    /// Zeigt das Belohnungs-Panel an und speichert die aktuelle MissionId.
    /// </summary>
    public void ShowReward(string missionName, string missionId)
    {
        Debug.Log("🌿 Zeige Weekly Mission Belohnung: " + missionName);

        if (panel == null)
        {
            Debug.LogError("Panel ist NULL!");
            return;
        }

        panel.SetActive(true);

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.Log("Keine CanvasGroup gefunden, füge hinzu.");
            cg = panel.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
        }
        else
        {
            cg.alpha = 0f; // Reset Alpha auf 0
        }

        currentMissionId = missionId;
        statusText.text = $"Mission abgeschlossen:\n<b>{missionName}</b>";
        collectButton.interactable = true;

        StartCoroutine(FadeInPanel());
    }

    void OnCollectClicked()
    {
        Debug.Log("🌿 Weekly Reward eingesammelt");

        collectButton.interactable = false;
        panel.SetActive(false);

        if (audioSource != null)
            audioSource.Play();

        // Belohnung speichern
        int stash = PlayerPrefs.GetInt("CannabisStash", 0) + cannabisReward;
        PlayerPrefs.SetInt("CannabisStash", stash);
        PlayerPrefs.Save();

        if (cannabisStashText != null)
            cannabisStashText.text = stash.ToString();

        // Markiere Reward als gesammelt im WeeklyMissionManager
        if (!string.IsNullOrEmpty(currentMissionId))
        {
            WeeklyMissionManager.Instance.OnRewardCollected(currentMissionId);
            currentMissionId = null;
        }

        StartCoroutine(AnimateRewardImage());
    }

    IEnumerator FadeInPanel()
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.LogError("CanvasGroup fehlt im FadeInPanel!");
            yield break;
        }

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        cg.alpha = 1f;
        Debug.Log("FadeInPanel beendet, Alpha = 1");
    }

    IEnumerator AnimateRewardImage()
    {
        Transform img = rewardImage.transform;
        Vector3 start = img.localPosition;
        Quaternion startRot = img.rotation;

        float height = 50f;
        float duration = 0.6f;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float h = 4 * height * t * (1 - t);
            img.localPosition = start + Vector3.up * h;
            img.rotation = startRot * Quaternion.Euler(0, 360 * t, 0);
            yield return null;
        }

        img.localPosition = start;
        img.rotation = startRot;
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
