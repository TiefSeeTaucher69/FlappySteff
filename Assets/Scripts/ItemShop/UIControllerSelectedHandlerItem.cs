using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIControllerSelectedHandlerItem : MonoBehaviour
{
    public GameObject[] possibleSelections;

    private GameObject lastSelected;

    void OnEnable()
    {
        StartCoroutine(DelayedSelect());
    }

    IEnumerator DelayedSelect()
    {
        yield return null; // 1 Frame warten, bis andere Scripts initialisiert sind

        GameObject toSelect = GetFirstValidSelection();

        if (toSelect != null)
        {
            Debug.Log("Ausgewählter Button für Controller (verzögert): " + toSelect.name);
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
