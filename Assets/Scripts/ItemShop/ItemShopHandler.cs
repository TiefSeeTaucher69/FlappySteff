// ItemShopHandler.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemShopHandler : MonoBehaviour
{
    // Stash
    public Text cannabisStashText;
    public Text cannabisStashTextPage2;
    public Text cannabisStashTextPage3;
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
    public int BlueTrailCost = 20;
    public Text blueTrailBuyText;
    public GameObject blueTrailBought;
    public Button blueTrailActivateButton;
    public Text blueTrailActivateButtonText;

    // Skins
    public int TomBirdCost = 25;
    public Text tomBirdBuyText;
    public GameObject tomBirdBought;
    public Button tomBirdActivateButton;
    public Text tomBirdActivateButtonText;

    public int BennetBirdCost = 25;
    public Text bennetBirdBuyText;
    public GameObject bennetBirdBought;
    public Button bennetBirdActivateButton;
    public Text bennetBirdActivateButtonText;

    public int BenjoBirdCost = 25;
    public Text benjoBirdBuyText;
    public GameObject benjoBirdBought;
    public Button benjoBirdActivateButton;
    public Text benjoBirdActivateButtonText;

    public int JanBirdCost = 25;
    public Text janBirdBuyText;
    public GameObject janBirdBought;
    public Button janBirdActivateButton;
    public Text janBirdActivateButtonText;

    // Steff Bird (default skin, always owned)
    public Button steffBirdActivateButton;
    public Text steffBirdActivateButtonText;

    void Start()
    {
        CannabisStash = PlayerPrefs.GetInt("CannabisStash", 0);
        cannabisStashText.text = CannabisStash.ToString();
        if (cannabisStashTextPage2 != null)
            cannabisStashTextPage2.text = CannabisStash.ToString();
        if (cannabisStashTextPage3 != null)
            cannabisStashTextPage3.text = CannabisStash.ToString();

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

        // Skins
        if (CannabisStash < TomBirdCost)
            tomBirdBuyText.color = Color.red;
        tomBirdBought.SetActive(PlayerPrefs.GetInt("HasSkin_tom-bird", 0) == 1);

        if (CannabisStash < BennetBirdCost)
            bennetBirdBuyText.color = Color.red;
        bennetBirdBought.SetActive(PlayerPrefs.GetInt("HasSkin_bennet-bird", 0) == 1);

        if (CannabisStash < BenjoBirdCost)
            benjoBirdBuyText.color = Color.red;
        benjoBirdBought.SetActive(PlayerPrefs.GetInt("HasSkin_benjo-bird", 0) == 1);

        if (CannabisStash < JanBirdCost)
            janBirdBuyText.color = Color.red;
        janBirdBought.SetActive(PlayerPrefs.GetInt("HasSkin_jan-bird", 0) == 1);

        // Wenn noch kein Skin aktiv ist, setze steff-bird als Standard
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("ActiveSkin")))
        {
            PlayerPrefs.SetString("ActiveSkin", "steff-bird");
            PlayerPrefs.Save();
        }

        UpdateActiveButtons();
    }

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

    public void BuyTomBird()
    {
        if (CannabisStash >= TomBirdCost && PlayerPrefs.GetInt("HasSkin_tom-bird", 0) == 0)
        {
            PlayerPrefs.SetInt("HasSkin_tom-bird", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - TomBirdCost);
            PlayerPrefs.Save();
            Start();
        }
    }

    public void BuyBennetBird()
    {
        if (CannabisStash >= BennetBirdCost && PlayerPrefs.GetInt("HasSkin_bennet-bird", 0) == 0)
        {
            PlayerPrefs.SetInt("HasSkin_bennet-bird", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - BennetBirdCost);
            PlayerPrefs.Save();
            Start();
        }
    }

    public void BuyBenjoBird()
    {
        if (CannabisStash >= BenjoBirdCost && PlayerPrefs.GetInt("HasSkin_benjo-bird", 0) == 0)
        {
            PlayerPrefs.SetInt("HasSkin_benjo-bird", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - BenjoBirdCost);
            PlayerPrefs.Save();
            Start();
        }
    }

    public void BuyJanBird()
    {
        if (CannabisStash >= JanBirdCost && PlayerPrefs.GetInt("HasSkin_jan-bird", 0) == 0)
        {
            PlayerPrefs.SetInt("HasSkin_jan-bird", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - JanBirdCost);
            PlayerPrefs.Save();
            Start();
        }
    }

    public void SelectSkin(string skinName)
    {
        // steff-bird ist immer verfügbar
        if (skinName == "steff-bird" || PlayerPrefs.GetInt("HasSkin_" + skinName, 0) == 1)
        {
            PlayerPrefs.SetString("ActiveSkin", skinName);
            PlayerPrefs.Save();
            UpdateActiveButtons();
        }
    }

    public void SelectActiveItem(string itemName)
    {
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
        string activeSkin = PlayerPrefs.GetString("ActiveSkin", "steff-bird");

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

        redTrailActivateButtonText.text = activeTrail == "TrailRed" ? "Active" : "Set Active";
        redTrailActivateButton.image.color = activeTrail == "TrailRed" ? Color.green : Color.red;

        purpleTrailActivateButtonText.text = activeTrail == "TrailPurple" ? "Active" : "Set Active";
        purpleTrailActivateButton.image.color = activeTrail == "TrailPurple" ? Color.green : Color.red;

        blueTrailActivateButtonText.text = activeTrail == "TrailBlue" ? "Active" : "Set Active";
        blueTrailActivateButton.image.color = activeTrail == "TrailBlue" ? Color.green : Color.red;

        tomBirdActivateButtonText.text = activeSkin == "tom-bird" ? "Active" : "Set Active";
        tomBirdActivateButton.image.color = activeSkin == "tom-bird" ? Color.green : Color.red;

        bennetBirdActivateButtonText.text = activeSkin == "bennet-bird" ? "Active" : "Set Active";
        bennetBirdActivateButton.image.color = activeSkin == "bennet-bird" ? Color.green : Color.red;

        benjoBirdActivateButtonText.text = activeSkin == "benjo-bird" ? "Active" : "Set Active";
        benjoBirdActivateButton.image.color = activeSkin == "benjo-bird" ? Color.green : Color.red;

        janBirdActivateButtonText.text = activeSkin == "jan-bird" ? "Active" : "Set Active";
        janBirdActivateButton.image.color = activeSkin == "jan-bird" ? Color.green : Color.red;

        // Steff Bird Button (default)
        steffBirdActivateButtonText.text = activeSkin == "steff-bird" ? "Active" : "Set Active";
        steffBirdActivateButton.image.color = activeSkin == "steff-bird" ? Color.green : Color.red;
    }
}
