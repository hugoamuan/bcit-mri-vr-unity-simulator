using UnityEngine;
/// <summary>
/// Toggles the active state of a set of buttons based on the active state of a trigger object.
/// </summary>
public class DisableButtons : MonoBehaviour
{
    // Array of main buttons that should be enabled/disabled based on the trigger's state
    public GameObject[] buttons;
    // if needed, use dummyButtons to replace the buttons when they are disabled
    public GameObject[] dummyButtons;
    // The trigger object that determines whether buttons are enabled or disabled
    public GameObject trigger;
    /// <summary>
    /// Called once when the script is first executed.
    /// Ensures buttons are in the correct state based on the trigger's initial state.
    /// </summary>
    void Start()
    {
        // Initialize button states based on trigger's active state
        SetButtonsActive(trigger.activeSelf);
    }
    /// <summary>
    /// Called once per frame. Continuously updates button states based on the trigger's state.
    /// </summary>
    void Update()
    {
        // Update button states dynamically
        SetButtonsActive(trigger.activeSelf);
    }

    /// <summary>
    /// Enables or disables the main buttons based on the trigger's active state.
    /// If the trigger is active, main buttons are shown and dummy buttons are hidden.
    /// If the trigger is inactive, main buttons are hidden and dummy buttons are shown.
    /// </summary>
    /// <param name="isActive">Determines whether the main buttons should be active.</param>
    private void SetButtonsActive(bool isActive)
    {
        // Loop through all main buttons and set their active state based on the trigger
        foreach (GameObject button in buttons)
        {
            button.SetActive(isActive);
        }
        // Loop through all dummy buttons and set them to the opposite active state
        foreach (GameObject dummyButton in dummyButtons)
        {
            dummyButton.SetActive(!isActive);
        }
    }
}
