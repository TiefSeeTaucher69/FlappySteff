using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ItemShopHandler : MonoBehaviour
{
    // Stash
    public Text cannabisStashText;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CannabisStash = PlayerPrefs.GetInt("CannabisStash", 0);
        cannabisStashText.text = CannabisStash.ToString();
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

    public void SelectActiveItem(string itemName)
    {
        // z. B. "Invincible" oder "Shrink" oder "Laser"
        PlayerPrefs.SetString("ActiveItem", itemName);
        PlayerPrefs.Save();
        UpdateActiveButtons();
    }

    void UpdateActiveButtons()
    {
        string activeItem = PlayerPrefs.GetString("ActiveItem", "");

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
    }

}
