using UnityEngine;

public class ShopPageSwitcher : MonoBehaviour
{
    public GameObject[] shopPages;
    private int currentPage = 0;

    public void NextPage()
    {
        shopPages[currentPage].SetActive(false);
        currentPage = (currentPage + 1) % shopPages.Length;
        shopPages[currentPage].SetActive(true);
    }

    public void PrevPage()
    {
        shopPages[currentPage].SetActive(false);
        currentPage = (currentPage - 1 + shopPages.Length) % shopPages.Length;
        shopPages[currentPage].SetActive(true);
    }
}
