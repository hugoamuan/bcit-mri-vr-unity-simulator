using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Toggles label visibility by enabling/disabling a secondary camera that renders only the Labels layer.
/// </summary>
public class LabelToggle : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("The main camera (usually Main Camera)")]
    public Camera mainCamera;
    
    [Header("Controller Input")]
    [Tooltip("Right hand controller button input action (Can be configured on the inspector(Secondary: B button for example))")]
    public InputActionReference rightControllerButtonAction;
    
    [Header("Debug")]
    public bool showDebug = true;
    
    private Camera labelCamera;
    private bool labelsVisible = false;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        CreateLabelCamera();
        
        if (rightControllerButtonAction != null)
        {
            rightControllerButtonAction.action.Enable();
            rightControllerButtonAction.action.performed += OnControllerButtonPressed;

        }
    }

    /// <summary>
    /// Creates a secondary camera parented to the main camera that only renders the Labels layer.
    /// </summary>
    void CreateLabelCamera()
    {
        GameObject labelCameraObj = new GameObject("LabelCamera");
        labelCameraObj.transform.SetParent(mainCamera.transform);
        labelCameraObj.transform.localPosition = Vector3.zero;
        labelCameraObj.transform.localRotation = Quaternion.identity;
        labelCameraObj.transform.localScale = Vector3.one;

        labelCamera = labelCameraObj.AddComponent<Camera>();
        
        labelCamera.fieldOfView = mainCamera.fieldOfView;
        labelCamera.nearClipPlane = mainCamera.nearClipPlane;
        labelCamera.farClipPlane = mainCamera.farClipPlane;
        
        labelCamera.cullingMask = LayerMask.GetMask("Labels");
        labelCamera.clearFlags = CameraClearFlags.Depth;
        labelCamera.depth = mainCamera.depth + 1;
        labelCamera.stereoTargetEye = mainCamera.stereoTargetEye;

        labelCamera.enabled = labelsVisible;
    }

    void OnDisable()
    {
        if (rightControllerButtonAction != null)
        {
            rightControllerButtonAction.action.performed -= OnControllerButtonPressed;
        }
    }

    /// <summary>
    /// Called when the controller button is pressed.
    /// </summary>
    private void OnControllerButtonPressed(InputAction.CallbackContext context)
    {
        ToggleLabels();
    }

    /// <summary>
    /// Toggles label visibility on/off.
    /// </summary>
    public void ToggleLabels()
    {
        labelsVisible = !labelsVisible;
        
        if (labelCamera != null)
        {
            labelCamera.enabled = labelsVisible;
        }
        
        if (showDebug)
        {
            Debug.Log($"Labels toggled: {(labelsVisible ? "On" : "Off")}");
        }
    }
}