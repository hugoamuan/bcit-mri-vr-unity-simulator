using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PatientClickInteractable : XRBaseInteractable
{
    public InputActionReference rightActivateAction;
    public InputActionReference leftActivateAction;
    public UnityEvent TriggerEvent;
    private int hoverCount = 0;

    void Start()
    {
        if (rightActivateAction != null)
        {
            rightActivateAction.action.Enable();
            rightActivateAction.action.performed += OnActivatePressed;
        }

        if (leftActivateAction != null)
        {
            leftActivateAction.action.Enable();
            leftActivateAction.action.performed += OnActivatePressed;
        }
    }


    void OnDisable()
    {
        if (rightActivateAction != null)
            rightActivateAction.action.performed -= OnActivatePressed;

        if (leftActivateAction != null)
            leftActivateAction.action.performed -= OnActivatePressed;
    }


    private void OnActivatePressed(InputAction.CallbackContext context)
    {
        if (hoverCount > 0)
        {
            TriggerEvent.Invoke();
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        hoverCount++;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        hoverCount = Mathf.Max(hoverCount - 1, 0);
    }
}
