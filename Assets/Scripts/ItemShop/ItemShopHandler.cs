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
    public GameObject invincibleItemBuyButton;
    public Text invincibleItemBuyText;
    public GameObject invincibleBought;
    public Button invincibleActivateButton;
    public Text invincibleActivateButtonText;

    // Shrink
    public int ShrinkItemCost = 50;
    public GameObject shrinkItemBuyButton;
    public Text shrinkItemBuyText;
    public GameObject shrinkBought;
    public Button shrinkActivateButton;
    public Text shrinkActivateButtonText;

    // Laser Shot
    public int LaserItemCost = 50;
    public GameObject laserItemBuyButton;
    public Text laserItemBuyText;
    public GameObject laserBought;
    public Button laserActivateButton;
    public Text laserActivateButtonText;

    // Red Trail
    public int RedTrailCost = 20;
    public GameObject redTrailBuyButton;
    public Text redTrailBuyText;
    public GameObject redTrailBought;
    public Button redTrailActivateButton;
    public Text redTrailActivateButtonText;

    // Purple Trail
    public int PurpleTrailCost = 20;
    public GameObject purpleTrailBuyButton;
    public Text purpleTrailBuyText;
    public GameObject purpleTrailBought;
    public Button purpleTrailActivateButton;
    public Text purpleTrailActivateButtonText;

    // Blue Trail
    public int BlueTrailCost = 20;
    public GameObject blueTrailBuyButton;
    public Text blueTrailBuyText;
    public GameObject blueTrailBought;
    public Button blueTrailActivateButton;
    public Text blueTrailActivateButtonText;

    // Skins
    public int TomBirdCost = 25;
    public GameObject tomBirdBuyButton;
    public Text tomBirdBuyText;
    public GameObject tomBirdBought;
    public Button tomBirdActivateButton;
    public Text tomBirdActivateButtonText;

    public int BennetBirdCost = 25;
    public GameObject bennetBirdBuyButton;
    public Text bennetBirdBuyText;
    public GameObject bennetBirdBought;
    public Button bennetBirdActivateButton;
    public Text bennetBirdActivateButtonText;

    public int GingerBirdCost = 25;
    public GameObject gingerBirdBuyButton;
    public Text gingerBirdBuyText;
    public GameObject gingerBirdBought;
    public Button gingerBirdActivateButton;
    public Text gingerBirdActivateButtonText;

    public int JanBirdCost = 25;
    public GameObject janBirdBuyButton;
    public Text janBirdBuyText;
    public GameObject janBirdBought;
    public Button janBirdActivateButton;
    public Text janBirdActivateButtonText;

    // Benjo Bird (default skin, always owned)
    public Button benjoBirdActivateButton;
    public Text benjoBirdActivateButtonText;

    void Start()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        CannabisStash = PlayerPrefs.GetInt("CannabisStash", 0);
        cannabisStashText.text = CannabisStash.ToString();
        if (cannabisStashTextPage2 != null)
            cannabisStashTextPage2.text = CannabisStash.ToString();
        if (cannabisStashTextPage3 != null)
            cannabisStashTextPage3.text = CannabisStash.ToString();

        // Invincible
        invincibleItemBuyText.color = CannabisStash < InvincibleItemCost ? Color.red : Color.white;
        bool hasInvincible = PlayerPrefs.GetInt("HasInvincibleItem", 0) == 1;
        invincibleBought.SetActive(hasInvincible);
        invincibleItemBuyButton.SetActive(!hasInvincible);

        // Shrink
        shrinkItemBuyText.color = CannabisStash < ShrinkItemCost ? Color.red : Color.white;
        bool hasShrink = PlayerPrefs.GetInt("HasShrinkItem", 0) == 1;
        shrinkBought.SetActive(hasShrink);
        shrinkItemBuyButton.SetActive(!hasShrink);

        // Laser
        laserItemBuyText.color = CannabisStash < LaserItemCost ? Color.red : Color.white;
        bool hasLaser = PlayerPrefs.GetInt("HasLaserItem", 0) == 1;
        laserBought.SetActive(hasLaser);
        laserItemBuyButton.SetActive(!hasLaser);

        // Red Trail
        redTrailBuyText.color = CannabisStash < RedTrailCost ? Color.red : Color.white;
        bool hasRedTrail = PlayerPrefs.GetInt("HasTrailRed", 0) == 1;
        redTrailBought.SetActive(hasRedTrail);
        redTrailBuyButton.SetActive(!hasRedTrail);

        // Purple Trail
        purpleTrailBuyText.color = CannabisStash < PurpleTrailCost ? Color.red : Color.white;
        bool hasPurpleTrail = PlayerPrefs.GetInt("HasTrailPurple", 0) == 1;
        purpleTrailBought.SetActive(hasPurpleTrail);
        purpleTrailBuyButton.SetActive(!hasPurpleTrail);

        // Blue Trail
        blueTrailBuyText.color = CannabisStash < BlueTrailCost ? Color.red : Color.white;
        bool hasBlueTrail = PlayerPrefs.GetInt("HasTrailBlue", 0) == 1;
        blueTrailBought.SetActive(hasBlueTrail);
        blueTrailBuyButton.SetActive(!hasBlueTrail);

        // Skins
        tomBirdBuyText.color = CannabisStash < TomBirdCost ? Color.red : Color.white;
        bool hasTom = PlayerPrefs.GetInt("HasSkin_tom-bird", 0) == 1;
        tomBirdBought.SetActive(hasTom);
        tomBirdBuyButton.SetActive(!hasTom);

        bennetBirdBuyText.color = CannabisStash < BennetBirdCost ? Color.red : Color.white;
        bool hasBennet = PlayerPrefs.GetInt("HasSkin_bennet-bird", 0) == 1;
        bennetBirdBought.SetActive(hasBennet);
        bennetBirdBuyButton.SetActive(!hasBennet);

        gingerBirdBuyText.color = CannabisStash < GingerBirdCost ? Color.red : Color.white;
        bool hasGinger = PlayerPrefs.GetInt("HasSkin_ginger-bird", 0) == 1;
        gingerBirdBought.SetActive(hasGinger);
        gingerBirdBuyButton.SetActive(!hasGinger);

        janBirdBuyText.color = CannabisStash < JanBirdCost ? Color.red : Color.white;
        bool hasJan = PlayerPrefs.GetInt("HasSkin_jan-bird", 0) == 1;
        janBirdBought.SetActive(hasJan);
        janBirdBuyButton.SetActive(!hasJan);

        // Default Skin (if not set)
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("ActiveSkin")))
        {
            PlayerPrefs.SetString("ActiveSkin", "benjo-bird");
            PlayerPrefs.Save();
        }

        UpdateActiveButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
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
            RefreshUI();
        }
    }

    public void BuyItemShrink()
    {
        if (CannabisStash >= ShrinkItemCost && PlayerPrefs.GetInt("HasShrinkItem", 0) == 0)
        {
            PlayerPrefs.SetInt("HasShrinkItem", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - ShrinkItemCost);
            PlayerPrefs.Save();
            RefreshUI();
        }
    }

    public void BuyItemLaser()
    {
        if (CannabisStash >= LaserItemCost && PlayerPrefs.GetInt("HasLaserItem", 0) == 0)
        {
            PlayerPrefs.SetInt("HasLaserItem", 1);
            PlayerPrefs.SetInt("CannabisStash", CannabisStash - LaserItemCost);
            PlayerPrefs.Save();
            RefreshUI();
        }
    }

    public void BuyRedTrail() { if (CannabisStash >= RedTrailCost && PlayerPrefs.GetInt("HasTrailRed", 0) == 0) { PlayerPrefs.SetInt("HasTrailRed", 1); PlayerPrefs.SetInt("CannabisStash", CannabisStash - RedTrailCost); PlayerPrefs.Save(); RefreshUI(); } }
    public void BuyPurpleTrail() { if (CannabisStash >= PurpleTrailCost && PlayerPrefs.GetInt("HasTrailPurple", 0) == 0) { PlayerPrefs.SetInt("HasTrailPurple", 1); PlayerPrefs.SetInt("CannabisStash", CannabisStash - PurpleTrailCost); PlayerPrefs.Save(); RefreshUI(); } }
    public void BuyBlueTrail() { if (CannabisStash >= BlueTrailCost && PlayerPrefs.GetInt("HasTrailBlue", 0) == 0) { PlayerPrefs.SetInt("HasTrailBlue", 1); PlayerPrefs.SetInt("CannabisStash", CannabisStash - BlueTrailCost); PlayerPrefs.Save(); RefreshUI(); } }
    public void BuyTomBird() { if (CannabisStash >= TomBirdCost && PlayerPrefs.GetInt("HasSkin_tom-bird", 0) == 0) { PlayerPrefs.SetInt("HasSkin_tom-bird", 1); PlayerPrefs.SetInt("CannabisStash", CannabisStash - TomBirdCost); PlayerPrefs.Save(); RefreshUI(); } }
    public void BuyBennetBird() { if (CannabisStash >= BennetBirdCost && PlayerPrefs.GetInt("HasSkin_bennet-bird", 0) == 0) { PlayerPrefs.SetInt("HasSkin_bennet-bird", 1); PlayerPrefs.SetInt("CannabisStash", CannabisStash - BennetBirdCost); PlayerPrefs.Save(); RefreshUI(); } }
    public void BuyGingerBird() { if (CannabisStash >= GingerBirdCost && PlayerPrefs.GetInt("HasSkin_ginger-bird", 0) == 0) { PlayerPrefs.SetInt("HasSkin_ginger-bird", 1); PlayerPrefs.SetInt("CannabisStash", CannabisStash - GingerBirdCost); PlayerPrefs.Save(); RefreshUI(); } }
    public void BuyJanBird() { if (CannabisStash >= JanBirdCost && PlayerPrefs.GetInt("HasSkin_jan-bird", 0) == 0) { PlayerPrefs.SetInt("HasSkin_jan-bird", 1); PlayerPrefs.SetInt("CannabisStash", CannabisStash - JanBirdCost); PlayerPrefs.Save(); RefreshUI(); } }

    public void SelectSkin(string skinName)
    {
        if (skinName == "benjo-bird" || PlayerPrefs.GetInt("HasSkin_" + skinName, 0) == 1)
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
        string activeSkin = PlayerPrefs.GetString("ActiveSkin", "benjo-bird");

        invincibleActivateButtonText.text = activeItem == "Invincible" ? "Active" : "Set Active";
        invincibleActivateButton.image.color = activeItem == "Invincible" ? Color.green : Color.red;

        shrinkActivateButtonText.text = activeItem == "Shrink" ? "Active" : "Set Active";
        shrinkActivateButton.image.color = activeItem == "Shrink" ? Color.green : Color.red;

        laserActivateButtonText.text = activeItem == "Laser" ? "Active" : "Set Active";
        laserActivateButton.image.color = activeItem == "Laser" ? Color.green : Color.red;

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

        gingerBirdActivateButtonText.text = activeSkin == "ginger-bird" ? "Active" : "Set Active";
        gingerBirdActivateButton.image.color = activeSkin == "ginger-bird" ? Color.green : Color.red;

        janBirdActivateButtonText.text = activeSkin == "jan-bird" ? "Active" : "Set Active";
        janBirdActivateButton.image.color = activeSkin == "jan-bird" ? Color.green : Color.red;

        benjoBirdActivateButtonText.text = activeSkin == "benjo-bird" ? "Active" : "Set Active";
        benjoBirdActivateButton.image.color = activeSkin == "benjo-bird" ? Color.green : Color.red;
    }
}
