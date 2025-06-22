using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemShopHandler : MonoBehaviour
{
    // Stash
    public Text cannabisStashText;
    public Text cannabisStashTextPage2; // <--- NEU
    public int CannabisStash;

    // Invincible
    public int InvincibleItemCost = 50;
    public Text invincibleItemBuyText;
    public GameObject invincibleBought;
    public Button invincibleActivateButton;
    public Text invincibleActivateButtonText;

    // Shrink
    public int ShrinkItemCost = 50;
    public Text shrinkItemBuyText;
    public GameObject shrinkBought;
    public Button shrinkActivateButton;
    public Text shrinkActivateButtonText;

    // Laser Shot
    public int LaserItemCost = 50;
    public Text laserItemBuyText;
    public GameObject laserBought;
    public Button laserActivateButton;
    public Text laserActivateButtonText;

    // Red Trail
    public int RedTrailCost = 20;
    public Text redTrailBuyText;
    public GameObject redTrailBought;
    public Button redTrailActivateButton;
    public Text redTrailActivateButtonText;

    // Purple Trail
    public int PurpleTrailCost = 20;
    public Text purpleTrailBuyText;
    public GameObject purpleTrailBought;
    public Button purpleTrailActivateButton;
    public Text purpleTrailActivateButtonText;

    // Blue Trail
    public int BlueTrailCost = 200;
    public Text blueTrailBuyText;
    public GameObject blueTrailBought;
    public Button blueTrailActivateButton;
    public Text blueTrailActivateButtonText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CannabisStash = PlayerPrefs.GetInt("CannabisStash", 0);
        cannabisStashText.text = CannabisStash.ToString();
        if (cannabisStashTextPage2 != null)
            cannabisStashTextPage2.text = CannabisStash.ToString();

        //Invincible Item
        if (CannabisStash < InvincibleItemCost)
        {
            invincibleItemBuyText.color = Color.red;
        }

        if (PlayerPrefs.GetInt("HasInvincibleItem", 0) == 1)
        {
            invincibleBought.SetActive(true);
        }
        else
        {
            invincibleBought.SetActive(false);
        }
        // Shrink Item
        if (CannabisStash < ShrinkItemCost)
        {
            shrinkItemBuyText.color = Color.red;
        }

        if (PlayerPrefs.GetInt("HasShrinkItem", 0) == 1)
        {
            shrinkBought.SetActive(true);
        }
        else
        {
            shrinkBought.SetActive(false);
        }
        // Laser Item
        if (CannabisStash < LaserItemCost)
        {
            laserItemBuyText.color = Color.red;
        }

        if (PlayerPrefs.GetInt("HasLaserItem", 0) == 1)
        {
            laserBought.SetActive(true);
        }
        else
        {
            laserBought.SetActive(false);
        }

        // Red Trail
        if (CannabisStash < RedTrailCost)
        {
            redTrailBuyText.color = Color.red;
        }
        redTrailBought.SetActive(PlayerPrefs.GetInt("HasTrailRed", 0) == 1);

        // Purple Trail
        if (CannabisStash < PurpleTrailCost)
        {
            purpleTrailBuyText.color = Color.red;
        }
        purpleTrailBought.SetActive(PlayerPrefs.GetInt("HasTrailPurple", 0) == 1);

        // Blue Trail
        if (CannabisStash < BlueTrailCost)
        {
            blueTrailBuyText.color = Color.red;
        }
        blueTrailBought.SetActive(PlayerPrefs.GetInt("HasTrailBlue", 0) == 1);

        UpdateActiveButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void BuyItemInvincible()
    {
        if (CannabisStash >= InvincibleItemCost && PlayerPrefs.GetInt("HasInvincibleItem", 0) == 0)
        {
            PlayerPrefs.SetInt("HasInvincibleItem", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - InvincibleItemCost);
            PlayerPrefs.Save();
            Start();
        }
        else
        {
            Debug.Log("Not enough cannabis stash to buy invincible item or already owned.");
            return;
        }
    }

    public void BuyItemShrink()
    {
        if (CannabisStash >= ShrinkItemCost && PlayerPrefs.GetInt("HasShrinkItem", 0) == 0)
        {
            PlayerPrefs.SetInt("HasShrinkItem", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - ShrinkItemCost);
            PlayerPrefs.Save();
            Start();
        }
        else
        {
            Debug.Log("Not enough cannabis stash to buy shrink item or already owned.");
            return;
        }
    }

    public void BuyItemLaser()
    {
        if (CannabisStash >= LaserItemCost && PlayerPrefs.GetInt("HasLaserItem", 0) == 0)
        {
            PlayerPrefs.SetInt("HasLaserItem", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - LaserItemCost);
            PlayerPrefs.Save();
            Start();
        }
        else
        {
            Debug.Log("Not enough cannabis stash to buy laser item or already owned.");
            return;
        }
    }

    public void BuyRedTrail()
    {
        if (CannabisStash >= RedTrailCost && PlayerPrefs.GetInt("HasTrailRed", 0) == 0)
        {
            PlayerPrefs.SetInt("HasTrailRed", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - RedTrailCost);
            PlayerPrefs.Save();
            Start();
        }
    }

    public void BuyPurpleTrail()
    {
        if (CannabisStash >= PurpleTrailCost && PlayerPrefs.GetInt("HasTrailPurple", 0) == 0)
        {
            PlayerPrefs.SetInt("HasTrailPurple", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - PurpleTrailCost);
            PlayerPrefs.Save();
            Start();
        }
    }

    public void BuyBlueTrail()
    {
        if (CannabisStash >= BlueTrailCost && PlayerPrefs.GetInt("HasTrailBlue", 0) == 0)
        {
            PlayerPrefs.SetInt("HasTrailBlue", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - BlueTrailCost);
            PlayerPrefs.Save();
            Start();
        }
    }

    public void SelectActiveItem(string itemName)
    {
        // z. B. "Invincible" oder "Shrink" oder "Laser"
        PlayerPrefs.SetString("ActiveItem", itemName);
        PlayerPrefs.Save();
        UpdateActiveButtons();
    }

    public void SelectActiveTrail(string trailName)
    {
        PlayerPrefs.SetString("ActiveTrail", trailName);
        PlayerPrefs.Save();
        UpdateActiveButtons();
    }

    void UpdateActiveButtons()
    {
        string activeItem = PlayerPrefs.GetString("ActiveItem", "");
        string activeTrail = PlayerPrefs.GetString("ActiveTrail", "");

        // Invincible Button prüfen
        if (activeItem == "Invincible")
        {
            invincibleActivateButtonText.text = "Active";
            invincibleActivateButton.image.color = Color.green;
        }
        else
        {
            invincibleActivateButtonText.text = "Set Active";
            invincibleActivateButton.image.color = Color.red;
        }

        // Shrink Button prüfen
        if (activeItem == "Shrink")
        {
            shrinkActivateButtonText.text = "Active";
            shrinkActivateButton.image.color = Color.green;
        }
        else
        {
            shrinkActivateButtonText.text = "Set Active";
            shrinkActivateButton.image.color = Color.red;
        }

        // Laser Button prüfen
        if (activeItem == "Laser")
        {
            laserActivateButtonText.text = "Active";
            laserActivateButton.image.color = Color.green;
        }
        else
        {
            laserActivateButtonText.text = "Set Active";
            laserActivateButton.image.color = Color.red;
        }

        // Trails
        redTrailActivateButtonText.text = activeTrail == "TrailRed" ? "Active" : "Set Active";
        redTrailActivateButton.image.color = activeTrail == "TrailRed" ? Color.green : Color.red;

        purpleTrailActivateButtonText.text = activeTrail == "TrailPurple" ? "Active" : "Set Active";
        purpleTrailActivateButton.image.color = activeTrail == "TrailPurple" ? Color.green : Color.red;

        blueTrailActivateButtonText.text = activeTrail == "TrailBlue" ? "Active" : "Set Active";
        blueTrailActivateButton.image.color = activeTrail == "TrailBlue" ? Color.green : Color.red;
    }
}
