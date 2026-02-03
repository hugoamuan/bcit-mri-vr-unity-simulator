using UnityEngine.EventSystems;
using UnityEngine;
/// <summary>
/// This class handles the interaction with a dial button, which triggers actions based on pointer events.
/// It implements both the <see cref="IPointerDownHandler"/> and <see cref="IPointerUpHandler"/> interfaces to detect when the button is pressed or released.
/// When pressed, it checks if the bed is at its minimum position and moves the bed a fixed distance if possible. 
/// It also tracks whether the button is being held down and updates the dial's position accordingly, using the <see cref="DialController"/>.
/// </summary>
public class DialButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // This variable is used to prevent the button from being pressed multiple times
    private bool canBePressed = true;
    // Reference to the BedController to control bed movement
    public BedController bedController;
    // The dial object that will be manipulated
    public GameObject dial;
    public GameObject Crosshair;
    // Flag to indicate when the button is being held down
    private bool isHolding = false; // Flag to indicate when the button is being held

    /// <summary>
    /// Called when the user presses the button.
    /// - If the button is allowed to be pressed or if the bed is at its minimum position, the bed moves a fixed distance.
    /// - Sets `isHolding` to `true` to indicate the button is being held.
    /// </summary>
    /// <param name="eventData">Pointer event data associated with the button press.</param>
public void OnPointerDown(PointerEventData eventData)
{
    // Check if the button can be pressed or if the bed is at its minimum position
    if (canBePressed || (canBePressed = bedController.IsAtMinX()))
    {
        // Prevent multiple presses
        canBePressed = false;
        // Move the bed a fixed distance
        bedController.MoveFixedDistance();
        Crosshair.GetComponent<Light>().enabled = false; // Disable the crosshair light
    }
    isHolding = true; // Start tracking hold
}
    /// <summary>
    /// Called when the user releases the button.
    /// - Sets `isHolding` to `false` to stop tracking the hold action.
    /// </summary>
    /// <param name="eventData">Pointer event data associated with the button release.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false; // Stop tracking hold
    }

    /// <summary>
    /// Updates the dial's position continuously while the button is being held.
    /// - Calls `DialDown()` when the button is held.
    /// - Calls `DialUp()` when the button is released.
    /// </summary>
    private void Update()
    {
        if (isHolding)
        {
            // Call DialDown() when the button is held
            dial.GetComponent<DialController>().DialDown();
        } else
        {
            // Call DialUp() when the button is released
            dial.GetComponent<DialController>().DialUp();
        }
    }
}