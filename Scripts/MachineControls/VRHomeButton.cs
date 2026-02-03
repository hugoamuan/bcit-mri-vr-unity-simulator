using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Extends <see cref="VRHoldButton"/> to handle the VR home button functionality.
/// When held, it moves the bed to the home position and turns on the corresponding button light.
/// </summary>
public class VRHomeButton : VRHoldButton
{
    /// <summary>
    /// Overrides the base `ifHeld()` method.
    /// - Calls `HomePosition()` on the `bedController` to move the bed.
    /// - Activates the button's light with `LightOn()`.
    /// </summary>
    protected override void ifHeld()
    {
        bedController.HomePosition(); // Move the bed to the home position
        LightOn(); // Turn on the button light
    }
}
