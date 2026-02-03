using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Helper script to trigger speech bubble from keyboard input or VR controller input.
/// Add this to any GameObject in your scene as a workaround for no speechbubblebuilder component being found in scene.
/// This was added to the game object SpeechBubbleTrigger in the scene.
/// Supports both keyboard (M key) and VR controller (X button on left hand Vive Focus 3) inputs.
/// </summary>
public class SpeechBubbleTriggerHelper : MonoBehaviour
{
    [Header("References")]
    [Tooltip("If not set, will automatically find PatientMenu")]
    public PatientMenu patientMenu;
    
    
    [Tooltip("Left hand controller button input action (X button on Vive Focus 3)")]
    public InputActionReference leftControllerButtonAction;
    
    
    /// <summary>
    /// Called when the script instance is being loaded.
    /// Automatically finds and assigns the PatientMenu if not manually assigned.
    /// Enables the left controller button action if assigned.
    /// Logs initialization status to the console for debugging.
    /// </summary>
    void Start()
    {
        // Auto-find PatientMenu if not set
        if (patientMenu == null)
        {
            patientMenu = FindFirstObjectByType<PatientMenu>();
            if (patientMenu != null)
            {
                Debug.Log("[SpeechBubbleTriggerHelper] Automatically found PatientMenu");
            }
            else
            {
                Debug.LogError("[SpeechBubbleTriggerHelper] Could not find PatientMenu in scene!");
            }
        }
        
        // Enable and subscribe to left controller button action if assigned
        if (leftControllerButtonAction != null)
        {
            leftControllerButtonAction.action.Enable();
            leftControllerButtonAction.action.performed += OnControllerButtonPressed;
            Debug.Log("[SpeechBubbleTriggerHelper] Left controller button action enabled");
        }
        else
        {
            Debug.LogWarning("[SpeechBubbleTriggerHelper] Left controller button action not assigned. Only keyboard input will work.");
        }
        
    }
    
    /// <summary>
    /// Called when the script is disabled.
    /// Unsubscribes from the left controller button action to prevent memory leaks.
    /// </summary>
    void OnDisable()
    {
        if (leftControllerButtonAction != null)
        {
            leftControllerButtonAction.action.performed -= OnControllerButtonPressed;
        }
    }

    
    /// <summary>
    /// Callback for when the left controller button (X button on Vive Focus 3) is pressed.
    /// Triggers the patient menu to show.
    /// </summary>
    /// <param name="context">The input action callback context containing button press information.</param>
    private void OnControllerButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("[SpeechBubbleTriggerHelper] Left controller button pressed!");
        TriggerMenu();
    }
    
    /// <summary>
    /// Triggers the patient menu to show/toggle.
    /// If PatientMenu reference exists, calls ShowMenu() to display patient interaction options.
    /// Logs an error if PatientMenu is not assigned.
    /// </summary>
    void TriggerMenu()
    {
        if (patientMenu != null)
        {
            Debug.Log("[SpeechBubbleTriggerHelper] Calling PatientMenu.ShowMenu()");
            patientMenu.ShowMenu();
        }
        else
        {
            Debug.LogError("[SpeechBubbleTriggerHelper] PatientMenu is not assigned!");
        }
    }
}
