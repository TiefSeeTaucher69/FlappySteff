using UnityEngine;
using UnityEngine.EventSystems;

public class UIControllerSelectedHandlerWeeklyReward : MonoBehaviour
{
    public GameObject firstSelected;

    private GameObject lastSelected;

    void OnEnable()
    {
        if (firstSelected != null && firstSelected.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
            lastSelected = firstSelected;
        }
    }

    void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current != null)
        {
            lastSelected = current;
        }

        if (current == null && IsUsingController())
        {
            if (lastSelected != null && lastSelected.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
            else if (firstSelected != null && firstSelected.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
                lastSelected = firstSelected;
            }
        }
    }

    private bool IsUsingController()
    {
        return Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f ||
               Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f ||
               Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel");
    }
}
