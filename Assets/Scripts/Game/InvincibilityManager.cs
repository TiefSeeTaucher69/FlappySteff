using UnityEngine;
using UnityEngine.UI; // Für TextMeshPro: using TMPro;
using System.Collections;

public class InvincibilityManager : MonoBehaviour
{
    public Collider2D playerCollider;
    public SpriteRenderer spriteRenderer;
    public Text cooldownText; // Oder TMP_Text für TextMeshPro

    public float invincibilityDuration = 2f;
    public float cooldownTime = 10f;
    public GameObject invincibilityUI;

    private bool isInvincible = false;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    private void Start()
    {

        if ( PlayerPrefs.GetInt("HasInvincibleItem", 0) == 1 && PlayerPrefs.GetString("ActiveItem", "") == "Invincible")
        {
            invincibilityUI.SetActive(true);
        } else
        {
            invincibilityUI.SetActive(false);
        }
    }
    void Update()
    {
        if (PlayerPrefs.GetString("ActiveItem", "") != "Invincible") return;
        HandleCooldownUI();

        if (Input.GetKeyDown(KeyCode.E) && !isInvincible && !isOnCooldown)
        {
            if (PlayerPrefs.GetInt("HasInvincibleItem", 0) == 1)
            {
                StartCoroutine(InvincibilityCoroutine());
            }
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }

    void HandleCooldownUI()
    {
        if (isInvincible)
        {
            cooldownText.text = "Unverwundbar!";
        }
        else if (isOnCooldown)
        {
            cooldownText.text = $"Bereit in: {cooldownTimer:F1}s";
        }
        else
        {
            cooldownText.text = "Bereit!";
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        playerCollider.enabled = false;

        float elapsed = 0f;
        float blinkInterval = 0.4f;

        while (elapsed < invincibilityDuration)
        {
            // Blinken durch Alpha
            Color color = spriteRenderer.color;
            color.a = (color.a == 1f) ? 0.3f : 1f;
            spriteRenderer.color = color;

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;

            blinkInterval = Mathf.Max(0.05f, blinkInterval * 0.8f);
        }

        // Zurücksetzen
        Color resetColor = spriteRenderer.color;
        resetColor.a = 1f;
        spriteRenderer.color = resetColor;

        playerCollider.enabled = true;
        isInvincible = false;

        // Jetzt startet der Cooldown!
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
    }
}
