using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LaserManager : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;
    public float cooldownTime = 5f;
    public Text cooldownText; // Anzeige wie "Bereit!" oder "Bereit in: X.Xs"
    public GameObject laserUI; // UI-Element aktivieren bei Itembesitz

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        // UI aktivieren nur wenn Item gekauft und ausgewählt
        if (PlayerPrefs.GetInt("HasLaserItem", 0) == 1 && PlayerPrefs.GetString("ActiveItem", "") == "Laser")
        {
            laserUI.SetActive(true);
        }
        else
        {
            laserUI.SetActive(false);
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetString("ActiveItem", "") != "Laser") return;

        HandleCooldownUI();

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) && !isOnCooldown)
        {
            if (PlayerPrefs.GetInt("HasLaserItem", 0) == 1)
            {
                FireLaser();
                StartCoroutine(StartCooldown());
            }
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 0f;
                isOnCooldown = false;
            }
        }
    }

    void FireLaser()
    {
        Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
    }

    IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
        yield return null;
    }

    void HandleCooldownUI()
    {
        if (isOnCooldown)
        {
            cooldownText.text = $"Bereit in: {cooldownTimer:F1}s";
        }
        else
        {
            cooldownText.text = "Bereit!";
        }
    }
}
