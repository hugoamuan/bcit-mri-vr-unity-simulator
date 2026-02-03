using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

// Handles click interaction on the canvas, toggling UI panels with animation.
public class ClickableCanvas : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Panel to Expand/Collapse")]
    public GameObject uiPanel;
    [Header("Objects to Close When Opening UI")]
    public GameObject[] closeOnOpen;
    [Header("Objects to Close When Closing UI")]
    public GameObject[] closeOnClose;
    // tracks if UI is open
    private static bool isOpen = false;

    [Header("Animation Settings")]
    private Vector3 expandedScale = new Vector3(5, 5, 1);  // Scale when expanded
    private Vector3 collapsedScale = new Vector3(1, 1, 1); // Scale when collapsed
    private float animationDuration = 0.3f; // Duration of scaling animation

    // Handles pointer click event to toggle UI
    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiPanel == null)
        {
            Debug.LogError("[ClickableCanvas] uiPanel is not assigned!");
            return;
        }
        // Toggle state
        isOpen = !isOpen;
        // Prevent overlapping animations
        StopAllCoroutines();
        // Start UI toggle
        StartCoroutine(AnimateAndToggle(isOpen));
    }

    // Animates scaling and toggles other UI elements accordingly
    private IEnumerator AnimateAndToggle(bool opening)
    {
        // Animation scaling
        yield return StartCoroutine(ScaleOverTime(opening ? expandedScale : collapsedScale));
        // Hide elements wnet opening
        setActive(closeOnOpen, !opening);
        // Show elements when opening
        setActive(closeOnClose, opening);
    }
    // Smoothly scales the panel over a set duration
    private IEnumerator ScaleOverTime(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        // Store the initial scale
        Vector3 startScale = uiPanel.transform.localScale;

        while (elapsedTime < animationDuration)
        {
            uiPanel.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure final scale is correct
        uiPanel.transform.localScale = targetScale; 
    }

    // Sets active state of multiple game objects safely
    private void setActive(GameObject[] objects, bool active)
    {
        if (objects == null) return; // Avoid null errors
        foreach (GameObject obj in objects)
        {
            if (obj != null) obj.SetActive(active); // Toggle visibility
        }
    }
}
