using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TabController : MonoBehaviour
{
    [System.Serializable]
    public class Tab
    {
        public Button button;
        public GameObject panel;
        public Image indicator; // schmaler Unterstrich unter dem Tab-Button
    }

    public Tab[] tabs;

    [Header("Navigations-Buttons (optional)")]
    public Button prevButton; // Btn_LEFT
    public Button nextButton; // Btn_RIGHT

    [Header("Farben")]
    public Color activeColor   = new Color(0.13f, 0.77f, 0.37f, 1f); // #22C55E
    public Color inactiveColor = new Color(0.33f, 0.33f, 0.33f, 1f); // #555555

    private int currentTab = 0;

    void Start()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i;
            tabs[i].button.onClick.AddListener(() => SwitchTab(index));
        }

        if (prevButton != null)
            prevButton.onClick.AddListener(VorigerTab);

        if (nextButton != null)
            nextButton.onClick.AddListener(NaechsterTab);

        SwitchTab(0);
    }

    void Update()
    {
        // Schultertasten direkt abfragen — berührt keine bestehenden Input Actions
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.leftShoulder.wasPressedThisFrame)  VorigerTab();
            if (gamepad.rightShoulder.wasPressedThisFrame) NaechsterTab();
        }
    }

    void VorigerTab()   => SwitchTab((currentTab - 1 + tabs.Length) % tabs.Length);
    void NaechsterTab() => SwitchTab((currentTab + 1) % tabs.Length);

    public void SwitchTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].panel.SetActive(false);
            if (tabs[i].indicator != null)
                tabs[i].indicator.color = inactiveColor;
        }

        currentTab = index;
        tabs[currentTab].panel.SetActive(true);

        if (tabs[currentTab].indicator != null)
            tabs[currentTab].indicator.color = activeColor;
    }
}
