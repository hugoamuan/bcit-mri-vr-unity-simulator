using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles Vive controller input to toggle the visibility of a floating image.
/// </summary>
public class ViveInputHandler : MonoBehaviour
{
    public InputActionReference rightControllerButtonAction;
    public FloatingImageController floatingImageController;

    private bool isImageVisible = false;

    private void OnEnable()
    {
        if (rightControllerButtonAction != null)
        {
            rightControllerButtonAction.action.performed += OnControllerButtonPressed;
            rightControllerButtonAction.action.Enable();
            Debug.Log("[ViveInputHandler] Action enabled and callback subscribed.");
        }
    }

    private void OnDisable()
    {
        if (rightControllerButtonAction != null)
        {
            rightControllerButtonAction.action.performed -= OnControllerButtonPressed;
            rightControllerButtonAction.action.Disable();
        }
    }

    private void OnControllerButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("[ViveInputHandler] Right controller primary button pressed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        ToggleImageVisibility();
    }

    private void ToggleImageVisibility()
    {
        isImageVisible = !isImageVisible;

        if (isImageVisible)
            floatingImageController.ShowImage();
        else
            floatingImageController.HideImage();
    }
}
