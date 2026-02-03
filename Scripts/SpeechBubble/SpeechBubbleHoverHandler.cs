using UnityEngine;
using UnityEngine.EventSystems;

public class SpeechBubbleHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PatientHoverInteractable hoverTarget;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverTarget?.HoverEnteredLogic();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTarget?.HoverExitedLogic();
    }
}
