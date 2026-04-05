using UnityEngine;
using UnityEngine.EventSystems;

// Dieses Script ersetzt den EventTrigger aus dem Plan.
// Auf jeden Button legen der die Dropdown-Zone triggern soll:
//   Maus:       IPointerEnterHandler / IPointerExitHandler
//   Controller: ISelectHandler / IDeselectHandler (New Input System kompatibel)
public class HoverDropdownTrigger : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{
    [SerializeField] private HoverDropdown controller;

    public void OnPointerEnter(PointerEventData _) => controller.OnEnterTrigger();
    public void OnPointerExit(PointerEventData _)  => controller.OnExitTrigger();
    public void OnSelect(BaseEventData _)           => controller.OnEnterTrigger();
    public void OnDeselect(BaseEventData _)         => controller.OnExitTrigger();
}
