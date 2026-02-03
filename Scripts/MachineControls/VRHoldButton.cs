using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Handles VR button interactions that require holding down the button.
/// Implements <see cref="IPointerDownHandler"/> and <see cref="IPointerUpHandler"/> to detect pointer events.
/// When the button is held, it continuously triggers an action, and when released, it stops.
/// Manages button light states via a <see cref="ButtonLightBase"/> controller.
/// </summary>
public class VRHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BedController bedController; // Reference to the bed controller to perform movement actions
    public ButtonLightBase buttonLightController; // Manages button light states
    public string type; // Button type
    protected bool isHeld = false; // Is the button held
    /// <summary>
    /// Continuously checks if the button is being held.
    /// - If held, triggers `ifHeld()`.
    /// - If not held, triggers `ifNotHeld()`.
    /// </summary>
    protected void Update()
    {
        if (isHeld)
        {
            ifHeld();
        }
        else
        {
            ifNotHeld();
        }
    }

    /// <summary>
    /// Virtual method executed when the button is held.
    /// - By default, turns on the button light.
    /// - Can be overridden in derived classes to perform additional actions.
    /// </summary>
    protected virtual void ifHeld()
    {
        LightOn();
    }

    /// <summary>
    /// Virtual method executed when the button is released.
    /// - By default, turns off the button light.
    /// - Can be overridden in derived classes to perform additional actions.
    /// </summary>
    protected virtual void ifNotHeld()
    {
        LightOff();
    }

    /// <summary>
    /// Turns on the corresponding button light.
    /// - Calls the `TurnButtonOn` method from the `ButtonLightBase` controller.
    /// </summary>
    protected void LightOn()
    {
        if (buttonLightController == null)
        {
            return;
        }
        buttonLightController.TurnButtonOn(type);
    }

    /// <summary>
    /// Turns off the corresponding button light.
    /// - Calls the `TurnButtonOff` method from the `ButtonLightBase` controller.
    /// </summary>
    protected void LightOff()
    {
        if (buttonLightController == null)
        {
            return;
        }
        buttonLightController.TurnButtonOff(type);
    }

    // Detects when the button is pressed and sets `isHeld` to true.
    public void OnPointerDown(PointerEventData eventData)
    {
        isHeld = true;
    }
    // Detects when the button is realsed and sets isHeld to false
    public void OnPointerUp(PointerEventData eventData)
    {
        isHeld = false;
    }
}
