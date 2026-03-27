using UnityEngine;
using UnityEngine.UI; // Für Text oder TMP_Text
using System.Collections;

public class ShrinkManager : MonoBehaviour
{
    public float shrinkDuration = 5f;
    public float cooldownTime = 10f;
    public Vector3 shrinkScale = new Vector3(0.5f, 0.5f, 1f);
    public Text cooldownText; // Zeigt Cooldown oder Status an
    public GameObject shrinkUI; // UI-Element, das du bei Besitz aktivierst

    private Vector3 originalScale;
    private bool isShrunk = false;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;
    private SteffScript steff;

    void Start()
    {
        originalScale = transform.localScale;
        steff = FindObjectOfType<SteffScript>();

        // UI nur aktivieren, wenn Item gekauft
        if (PlayerPrefs.GetInt("HasShrinkItem", 0) == 1 && PlayerPrefs.GetString("ActiveItem", "") == "Shrink")
        {
            shrinkUI.SetActive(true);
        }
        else
        {
            shrinkUI.SetActive(false);
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetString("ActiveItem", "") != "Shrink") return;
        HandleCooldownUI();

        if (steff != null && !steff.steffIsAlive) return;

        if ((Input.GetKeyDown(KeyCode.E) && !isShrunk && !isOnCooldown) ||
            (Input.GetKeyDown(KeyCode.Mouse0) && !isShrunk && !isOnCooldown) ||
            (Input.GetKeyDown(KeyCode.JoystickButton3) && !isShrunk && !isOnCooldown))
        {
            if (PlayerPrefs.GetInt("HasShrinkItem", 0) == 1)
            {
                StartCoroutine(ActivateShrink());
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

    IEnumerator ActivateShrink()
    {
        isShrunk = true;
        transform.localScale = shrinkScale;

        yield return new WaitForSeconds(shrinkDuration);

        transform.localScale = originalScale;
        isShrunk = false;

        // Cooldown aktivieren
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
    }

    void HandleCooldownUI()
    {
        if (isShrunk)
            cooldownText.text = "Geschrumpft!";
        else if (isOnCooldown)
            cooldownText.text = $"Bereit in: {cooldownTimer:F1}s";
        else
            cooldownText.text = "Bereit!";
    }
}
