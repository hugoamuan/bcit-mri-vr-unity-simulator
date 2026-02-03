using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages all HintAReas in the scene, tracking which item is held
/// and showing hints accordingly based on the current exam type.
/// </summary>
public class HintManager : MonoBehaviour
{
    public static HintManager Instance; // Singleton reference
    public string currentExamType; // The current active exam
    public List<HintArea> allHintAreas = new List<HintArea>(); // All registered hint areas

    private HoldableItem currentHeldItem; // Currently held item

    private void Awake()
    {
        // Set singleton instance
        Instance = this;
    }

    /// <summary>
    /// Called when a HoldableItem is picked up
    /// - Updates held item and refreshes hints
    /// </summary>
    /// <param name="item"></param>
    public void SetHeldItem(HoldableItem item)
    {
        currentHeldItem = item;
        currentExamType = DataBanker.Instance.GetExamType(); // <-- update exam type here
        Debug.Log("SetHeldItem called for: " + item.itemType + " with exam: " + currentExamType);
        UpdateHints();
    }

    /// <summary>
    /// Called when a HoldableItem is released
    /// - Clears held item and hides all hints.
    /// </summary>
    public void ClearHeldItem()
    {
        currentHeldItem = null;
        Debug.Log("Held item cleared");
        UpdateHints();
    }

    /// <summary>
    /// Checks all registered HintAreas and shows the correct hints
    /// based on the current held item and exam type.
    /// </summary>
    private void UpdateHints()
    {
        HideAllHints();

        if (currentHeldItem == null)
        {
            Debug.Log("No held item, skipping hints.");
            return;
        }

        Debug.Log("Checking hints for " + currentHeldItem.itemType + " and exam: " + currentExamType);

        foreach (var area in allHintAreas)
        {
            bool itemMatch = area.IsForItem(currentHeldItem.itemType);
            bool examMatch = area.IsForExam(currentExamType);

            Debug.Log($"HintArea {area.name} | ItemMatch: {itemMatch} | ExamMatch: {examMatch}");

            if (itemMatch && examMatch)
            {
                Debug.Log("Showing hint for " + area.name);
                area.ShowHint();
            }
        }
    }

    /// <summary>
    /// Refreshes the exam type from DataBanker and hides all hints
    /// </summary>
    public void RefreshExamType()
    {
        currentExamType = DataBanker.Instance.GetExamType();
        HideAllHints();
    }

    /// <summary>
    /// Hides all registered HintAreas
    /// </summary>
    private void HideAllHints()
    {
        foreach (var area in allHintAreas)
            area.HideHint();
    }
}
