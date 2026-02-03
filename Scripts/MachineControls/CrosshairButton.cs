using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// This class represents a button in the UI that toggles the spotlight on or off when clicked.
/// It implements the <see cref="IPointerClickHandler"/> interface to respond to click events.
/// When the button is clicked, it calls the <see cref="SpotlightController.ToggleSpotlight"/> method to toggle the spotlight's state.
/// </summary>
public class CrosshairButton : MonoBehaviour, IPointerClickHandler
{
    // Reference to the SpotlightController that manages the spotlight's behavior
    public SpotlightController spotlightController;
    /// <summary>
    /// This method is triggered when the button is clicked.
    /// It calls the ToggleSpotlight method to turn the spotlight on or off.
    /// </summary>
    /// <param name="eventData">Contains information about the click event.</param>

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the spotlight controller is assigned before attempting to toggle the spotlight
        if (spotlightController != null)
        {
            spotlightController.ToggleSpotlight();
        }
        else
        {
            Debug.LogWarning("SpotlightController reference is missing on CrosshairButton!");
        }
    }
}