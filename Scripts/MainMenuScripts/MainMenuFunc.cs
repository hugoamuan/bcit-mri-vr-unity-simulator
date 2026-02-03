using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // For TextMeshPro support
using UnityEngine.EventSystems; // Required for EventSystem
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem.XR; // Required for XR Input Reset
using UnityEngine.XR.Management;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

// Handles the main menu interactions, UI switching, and XR teleportation
public class MainMenuFunc : MonoBehaviour
{
    // Assign XR Origin in Inspector
    public XROrigin currentXROrigin;
    // Initial UI Canvas
    public GameObject canvas1;
    public GameObject canvas2;

    private void Awake()
    {
        // Any initialization can go here
    }

    // Switches between the two UI canvases with a slight delay to prevent UI glitches.
    public void SwitchCanvas()
    {
        if (canvas1 == null || canvas2 == null)
        {
            Debug.LogError("One or both canvas objects are not assigned!");
            return;
        }
        Invoke(nameof(ToggleCanvas), 0.1f); // Small delay to avoid UI glitches
    }

    // Toggles visibility of both canvases.
    private void ToggleCanvas()
    {
        canvas1.SetActive(!canvas1.activeSelf);
        canvas2.SetActive(!canvas2.activeSelf);
    }

    // Handles button clicks, determines action based on button text.
    public void OnButtonClicked(Button clickedButton)
    {
        if (clickedButton == null)
        {
            Debug.LogError("No button was clicked!");
            return;
        }

        Debug.Log("Button Clicked: " + clickedButton.name);

        // Fetch and validate button text.
        TextMeshProUGUI buttonTextComponent = clickedButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonTextComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in button children!");
            return;
        }

        string buttonText = buttonTextComponent.text;
        Debug.Log("Button Text: " + buttonText);
        // Check if the button text corresponds to gender selection.
        if (buttonText == "Male" || buttonText == "Female")
        {
            // Store selected gender
            DataBanker.Instance.SetGender(buttonText);
            // Switch UI after selection
            SwitchCanvas();
        }
        else
        {
            // Handle the exam selection
            examCheck(buttonText);
        }
    }

    // Stores the selected exam type and teleports the XR rig to its starting position.
    public void examCheck(string check)
    {
        Debug.Log("Exam Check called with: " + check);
        DataBanker.Instance.SetExamType(check);

        // Teleport the current XR Rig to position (0, 0, 0)
        TeleportCurrentXRToOrigin();

        if (HintManager.Instance != null)
        {
            HintManager.Instance.RefreshExamType();
            Debug.Log("HintManager updated with exam type: " + DataBanker.Instance.GetExamType());
        }
        else
        {
            Debug.LogWarning("HintManager instance not found.");
        }
    }
    // Moves the XR Rig to a predefined starting position and orientation.
    private void TeleportCurrentXRToOrigin()
    {
        if (currentXROrigin != null)
        {
            // Teleport the XR Origin to a new position
            currentXROrigin.transform.position = new Vector3(5, 0, 0);
            Debug.Log("XR Rig teleported to (5, 0, 0)");
			currentXROrigin.transform.rotation = Quaternion.Euler(0, -90, 0);
            Debug.Log("XR Rig teleported to (5, 0, 0) facing -90 degrees.");
        }
        else
        {
            Debug.LogError("Current XROrigin not found for teleportation!");
        }
    }
}