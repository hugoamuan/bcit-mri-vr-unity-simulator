using UnityEngine;

/// <summary>
/// This script manages snap conditions for knee coil parts.
/// Bottom knee coil can be placed anytime.
/// Top knee coil can only be placed after the patient is laying down.
/// </summary>
public class KneeCoilSnapCondition : MonoBehaviour, ISnapCondition
{
    public PatientStateManager patientStateManager;
    public DataBanker dataBanker;
    
    [Tooltip("Set to true for the top/upper knee coil part, false for the bottom part")]
    public bool isTopCoilPart = false;
    
    /// <summary>
    /// Checks if the snap is allowed based on the patient's current state and coil part type.
    /// Bottom coil: Can always be placed (no restriction).
    /// Top coil: Only allows snapping when the patient is laying down (kneeLowered state).
    /// For other exam types, always allows snapping.
    /// </summary>
    /// <returns>True if snapping is allowed, false otherwise.</returns>
    public bool IsSnapAllowed()
    {
        // Only apply this condition for knee exams
        if (dataBanker.GetExamType() != "Knee")
        {
            return true;
        }

        // Bottom coil part can always be placed
        if (!isTopCoilPart)
        {
            return true;
        }

        // For top coil part, check patient state
        // Check if patient state manager is assigned
        if (patientStateManager == null)
        {
            Debug.LogWarning("PatientStateManager is not assigned to KneeCoilSnapCondition!");
            return true; // Default to allowing snap if not configured
        }

        // Get the current patient state
        PatientState currentState = patientStateManager.GetCurrentState();
        
        // If no state is set yet, don't allow snapping the top part
        if (currentState == null)
        {
            return false;
        }

        // Only allow snapping top part when patient is laying down with knee lowered
        return currentState.label == "kneeLowered";
    }

    /// <summary>
    /// Returns a message to display when snapping is prevented.
    /// </summary>
    /// <returns>A user-friendly message explaining why snapping is not allowed.</returns>
    public string GetRefusalMessage()
    {
        if (isTopCoilPart)
        {
            return "Place the bottom knee coil first, then have the patient lay down before placing the top knee coil.";
        }
        else
        {
            return "Bottom knee coil cannot be placed at this time.";
        }
    }
}

// <summary>
// Interface for snap conditions that determine if an object can be snapped to a snap point.
// </summary>
public interface ISnapCondition
{
    // <summary>
    // Determines if the snap action is allowed.
    // </summary>
    // <returns>True if snap is allowed, false otherwise.</returns>
    bool IsSnapAllowed();

    // <summary>
    // Gets a message explaining why the snap was refused.
    // </summary>
    // <returns>A user-friendly refusal message.</returns>
    string GetRefusalMessage();
}
