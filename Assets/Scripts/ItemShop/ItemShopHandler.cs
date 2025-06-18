using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ItemShopHandler : MonoBehaviour
{
    public Text cannabisStashText;
    public int CannabisStash;
    public int InvincibleItemCost = 50;
    public Text invincibleItemBuyText;
    public GameObject invincibleBought;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CannabisStash = PlayerPrefs.GetInt("CannabisStash", 0);
        cannabisStashText.text = CannabisStash.ToString();
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
            Start();
        }
        else
        {
            Debug.Log("Not enough cannabis stash to buy invincible item or already owned.");
            return;
        }
    }
}
