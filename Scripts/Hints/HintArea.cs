using System;
using UnityEngine;

/// <summary>
/// Represents a "hint zone" in the scene that can highlight an area
/// - When a matching item is picked up and the exam type matches.
/// </summary>
public class HintArea : MonoBehaviour
{
    [Header("Hint Configuration")]
    public string itemType;            // e.g. "Blanket"
    public string[] examTypes;         // e.g. { "Head", "Knee" }
    public GameObject highlightObject; // Assign BlanketHighlightGlow prefab here

    private void Start()
    {
        // Register this HintArea with the HintManager
        if (HintManager.Instance != null)
        {
            HintManager.Instance.allHintAreas.Add(this);
            Debug.Log("Registered HintArea: " + name);
        }
        else
        {
            Debug.LogWarning("HintManager instance not found while registering " + name);
        }

        // Make sure highlight starts hidden
        if (highlightObject != null)
            highlightObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // Unregister from HintManager when destroyed
        if (HintManager.Instance != null)
            HintManager.Instance.allHintAreas.Remove(this);
    }
    /// <summary>
    /// Returns true if the held item matches this HintArea's itemType
    /// </summary>
    /// <param name="heldItemType">Current item the user is holding</param>
    /// <returns></returns>
    public bool IsForItem(string heldItemType)
    {
        return heldItemType == itemType;
    }

    /// <summary>
    /// Returns true if the current exam is one of the examTypes the HintArea
    /// is configured for
    /// </summary>
    /// <param name="currentExamType">The current exam type, updates whenever the user picks up an item.</param>
    /// <returns></returns>
    public bool IsForExam(string currentExamType)
    {
        return examTypes != null
        && examTypes.Length > 0
        && Array.Exists(examTypes, type => type == currentExamType);
    }

    /// <summary>
    /// Makes the highlighted area visible.
    /// </summary>
    public void ShowHint()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(true);
            Debug.Log("Hint shown for " + name);
        }
    }

    /// <summary>
    /// Hides the highlight
    /// </summary>
    public void HideHint()
    {
        if (highlightObject != null)
            highlightObject.SetActive(false);
    }
}
