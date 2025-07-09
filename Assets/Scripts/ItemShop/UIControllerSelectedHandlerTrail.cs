using UnityEngine;
using UnityEngine.EventSystems;

public class UIControllerSelectedHandlerTrail : MonoBehaviour
{
    public GameObject[] possibleSelections;

    private GameObject lastSelected;

    void OnEnable()
    {
        GameObject toSelect = GetFirstValidSelection();

        if (toSelect != null)
        {
            EventSystem.current.SetSelectedGameObject(toSelect);
            lastSelected = toSelect;
        }
        else
        {
            Debug.LogWarning("Kein aktives UI-Element gefunden für Auswahl.");
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
            GameObject toSelect = lastSelected != null && lastSelected.activeInHierarchy
                ? lastSelected
                : GetFirstValidSelection();

            if (toSelect != null)
            {
                EventSystem.current.SetSelectedGameObject(toSelect);
                lastSelected = toSelect;
            }
        }
    }

    private GameObject GetFirstValidSelection()
    {
        foreach (GameObject obj in possibleSelections)
        {
            if (obj != null && obj.activeInHierarchy)
                return obj;
        }
        return null;
    }

    private bool IsUsingController()
    {
        return Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f ||
               Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f ||
               Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel");
    }
}
