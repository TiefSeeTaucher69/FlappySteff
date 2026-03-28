using UnityEngine;
using UnityEngine.UI;

public class ShopPageSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class ShopPage
    {
        public GameObject panel;
        public Image      indicator; // Img_Indicator_ITEMS etc.
    }

    public ShopPage[] pages;

    public Color activeColor   = new Color(0.13f, 0.77f, 0.37f, 1f); // #22C55E
    public Color inactiveColor = new Color(0.33f, 0.33f, 0.33f, 1f);

    private int currentPage = 0;

    void Start() => SwitchToPage(0);

    public void SwitchToPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].panel.SetActive(false);
            if (pages[i].indicator != null)
                pages[i].indicator.color = inactiveColor;
        }

        currentPage = index;
        pages[currentPage].panel.SetActive(true);

        if (pages[currentPage].indicator != null)
            pages[currentPage].indicator.color = activeColor;
    }

    // Für die < > Pfeil-Buttons
    public void NextPage() => SwitchToPage((currentPage + 1) % pages.Length);
    public void PrevPage() => SwitchToPage((currentPage - 1 + pages.Length) % pages.Length);
}
