using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
/// Handles XR interactions for bed control buttons on the Bed Control Panel, managing visual feedback and continuous input while held.
/// </summary>
public class BedButtonInteraction : MonoBehaviour {
    public BedController bedController;
    public string buttonType;

    private Renderer buttonRenderer;
    private Color originalColor;
    private Color pressedColor = new Color(1f, 0.5f, 0f, 1f);
    private bool isHeld = false;


    /// <summary>
    /// Initializes the button renderer and stores its original color for visual feedback.
    /// </summary>
    void Start() {
        buttonRenderer = GetComponent<Renderer>();
        if (buttonRenderer != null) {
            originalColor = buttonRenderer.material.color;
        }
    }

    /// <summary>
    /// Registers XR select and deselect event listeners(for Grip button) to the provided interactable component.
    /// </summary>
    public void SetupXREvents(XRSimpleInteractable interactable) {
        interactable.selectEntered.AddListener(OnXRSelect);
        interactable.selectExited.AddListener(OnXRDeselect);
    }

    /// <summary>
    /// Continuously executes the button action each frame while the button is held down.
    /// Press and hold Grip button to trigger this
    /// </summary>
    void Update() {
        if (isHeld && bedController != null) {
            ExecuteButtonAction();
        }
    }

    /// <summary>
    /// Handles XR selection by setting the held state to true and changing the button color to pressed state.
    /// Press Grip button to trigger this
    /// </summary>
    private void OnXRSelect(SelectEnterEventArgs args) {
        isHeld = true;
        if (buttonRenderer != null) {
            buttonRenderer.material.color = pressedColor;
        }
    }

    /// <summary>
    /// Handles XR deselection by releasing the held state and restoring the button's original color.
    /// </summary>
    private void OnXRDeselect(SelectExitEventArgs args) {
        isHeld = false;
        if (buttonRenderer != null) {
            buttonRenderer.material.color = originalColor;
        }
    }


    /// <summary>
    /// Executes the appropriate bed controller action based on the button type (HeightUp, HeightDown, TrayIn, TrayOut).
    /// </summary>
    void ExecuteButtonAction() {
        switch (buttonType) {
            case "HeightUp":
                bedController.MoveUp();  
                break;
            case "HeightDown":
                bedController.MoveDown();  
                break;
            case "TrayIn":
                bedController.MoveUp(); 
                break;
            case "TrayOut":
                bedController.MoveDown();  
                break;
        }
    }
}