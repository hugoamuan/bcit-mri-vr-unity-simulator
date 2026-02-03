using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// This class inherits from the BasicLightButton class and implements the IPointerClickHandler interface.
/// It controls a toggle button that turns a light on or off when clicked. 
/// The button's state alternates between on and off each time it is clicked, using the TurnOn and TurnOff methods inherited from BasicLightButton.
/// </summary>
public class ToggleLightButton : BasicLightButton, IPointerClickHandler
{
    /// Handles pointer click events to toggle the button state.
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"ToggleLightButton clicked: {buttonName}");
        if (isOn)
        {
            Debug.Log($"Turning OFF the light: {buttonName}");
            TurnOff();
        }
        else
        {
            Debug.Log($"Turning ON the light: {buttonName}");
            TurnOn();
        }
    }
}
