using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Attach this script to any object that the user may pickup.
/// - Notifies the HintManager when HoldableItem is picked up or released.
/// </summary>
public class HoldableItem : MonoBehaviour
{
    public string itemType; // e.g. "Blanket"

    /// <summary>
    /// Called by XR Interaction toolkit, when the object is grabbed.
    /// </summary>
    /// <param name="args"></param>
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log(itemType + " was picked up!" + DataBanker.Instance.GetExamType());
        HintManager.Instance.SetHeldItem(this);
    }

    /// <summary>
    /// Called by XR Interaction Toolkit when the object is released.
    /// </summary>
    /// <param name="args"></param>
    public void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log(itemType + " was released!");
        HintManager.Instance.ClearHeldItem();
    }
}
