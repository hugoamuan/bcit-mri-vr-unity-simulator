using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// This class handles the XR interaction for a grabable object, specifically for a handle.
/// It manages the position and rotation of the handle when grabbed, as well as turning a the handle button light on/off.
/// The handle can be attached to a custom parent for specific transformations during interaction.
/// </summary>
public class HandleXRGrabInteraction : XRGrabInteractable
{
    // Controls the button light
    public ButtonLightController buttonLightController;
    // Name of the button light
    public string buttonName;
    // Custom parent transform
    public Transform customParent;
    // Tracks grab state
    private bool isGrabbed = false;
    // Stores local position when grabbed
    private Vector3 localPosition;
    // Stores local rotation when grabbed
    private Quaternion localRotation;
    /// <summary>
    /// Called when the handle is selected (grabbed) by an XR controller.
    /// It turns on the button light and stores the local position and rotation for later use.
    /// </summary>
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Call the base class method to handle default XR interaction
        base.OnSelectEntered(args);

        if (buttonLightController != null)
        {
            buttonLightController.TurnButtonOn(buttonName); // Turn on the assigned button light
            Debug.Log($"Button '{buttonName}' turned ON."); // Log the button light activation
        }
        else
        {
            Debug.LogWarning($"ButtonLightController not assigned for '{gameObject.name}'."); // Warning if no controller is assigned
        }

        isGrabbed = true; // Set grab state to true
        Debug.Log($"isGrabbed set to true for '{gameObject.name}'."); // Log grab state change

        // Determine which parent to use (custom parent if assigned, otherwise default)
        Transform parentToUse = customParent;

        if (parentToUse != null)
        {
            localPosition = parentToUse.InverseTransformPoint(transform.position);
            localRotation = Quaternion.Inverse(parentToUse.rotation) * transform.rotation;
            Debug.Log($"Stored localPosition: {localPosition}, localRotation: {localRotation}"); // Log stored values
        }
        else
        {
            Debug.LogWarning($"Custom parent not assigned for '{gameObject.name}', using default transform."); // Warning if no custom parent exists
        }
    }
    /// <summary>
    /// Called when the handle is deselected (released) by an XR controller.
    /// It turns off the button light and reattaches the object to the original or custom parent.
    /// It also applies the stored local position and rotation to avoid snapping back.
    /// </summary>
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Call the base method for default XR behavior
        base.OnSelectExited(args);

        if (buttonLightController != null)
        {
            buttonLightController.TurnButtonOff(buttonName); // Turn off the assigned button
            Debug.Log($"Button '{buttonName}' turned OFF."); // Log the button light deactivation
        }
        else
        {
            Debug.LogWarning($"ButtonLightController not assigned for '{gameObject.name}'."); // Warning if no controller is assigned
        }

        isGrabbed = false; // Reset grab state

        // Reattach the object to the custom parent or the original parent if not set
        if (customParent != null)
        {
            transform.SetParent(customParent, true); // Set the parent transform while keeping world position
            
        }
        else if (transform.parent != null)
        {
            transform.SetParent(transform.parent, true);
        }

        // Apply the stored local position and rotation to avoid snapping back
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
    }
    /// <summary>
    /// Continuously updates the position and rotation of the handle when it is being grabbed.
    /// It ensures the handle moves according to the custom or original parent’s transformation.
    /// </summary>
    private void Update()
    {
        if (isGrabbed)
        {
            // Use the custom parent or original parent (fallback if customParent isn't set)
            Transform parentToUse = customParent ? customParent : transform.parent;

            if (parentToUse != null)
            {
                // Update the position and rotation continuously
                transform.position = parentToUse.TransformPoint(localPosition);
                transform.rotation = parentToUse.rotation * localRotation;
            }
        }
    }

    /// <summary>
    /// Returns whether the handle is currently grabbed.
    /// </summary>
    public bool isHandleGrabbed()
    {
        return isGrabbed;
    }
}
