using UnityEngine;

/// <summary>
/// Snap condition for head coil parts. Bottom head coil can be placed anytime.
/// Top head coil can only be placed when the patient is in the laying-down state ("lyingHeadToScanner").
/// </summary>
public class HeadCoilSnapCondition : MonoBehaviour, ISnapCondition
{
    public PatientStateManager patientStateManager;
    public DataBanker dataBanker;

    [Tooltip("Set to true for the top/upper head coil part, false for the bottom part")]
    public bool isTopCoilPart = false;

    public bool IsSnapAllowed()
    {
        // Only apply this condition for head exams
        if (dataBanker.GetExamType() != "Head")
        {
            return true;
        }

        // Bottom head coil part can always be placed
        if (!isTopCoilPart)
        {
            return true;
        }

        if (patientStateManager == null)
        {
            Debug.LogWarning("PatientStateManager is not assigned to HeadCoilSnapCondition!");
            return true; // default to allowing snap if not configured
        }

        PatientState currentState = patientStateManager.GetCurrentState();
        if (currentState == null) return false;

        // Allow only when patient state is "lyingHeadToScanner"
        return currentState.label == "lyingHeadToScanner";
    }

    public string GetRefusalMessage()
    {
        if (isTopCoilPart)
        {
            return "Place the bottom head coil first, then have the patient lay down before placing the top head coil.";
        }
        else
        {
            return "Bottom head coil cannot be placed at this time.";
        }
    }
}
