using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TriggerOrGrab : XRBaseInteractable
{
    public InputActionReference rightActivateAction;
    public InputActionReference leftActivateAction;
    public UnityEvent TriggerOrGrabEvent;
    private XRBaseControllerInteractor hoveringInteractor = null;

    void Start()
    {
        if (rightActivateAction != null)
        {
            rightActivateAction.action.Enable();
            rightActivateAction.action.performed += OnRightActivatePressed;
        }

        if (leftActivateAction != null)
        {
            leftActivateAction.action.Enable();
            leftActivateAction.action.performed += OnLeftActivatePressed;
        }
    }

    void OnDisable()
    {
        if (rightActivateAction != null)
            rightActivateAction.action.performed -= OnRightActivatePressed;

        if (leftActivateAction != null)
            leftActivateAction.action.performed -= OnLeftActivatePressed;
    }

    private void OnRightActivatePressed(InputAction.CallbackContext context)
    {
        if (hoveringInteractor != null && hoveringInteractor.gameObject.CompareTag("RightController"))
        {
            TriggerOrGrabEvent.Invoke();
        }
    }

    private void OnLeftActivatePressed(InputAction.CallbackContext context)
    {
        if (hoveringInteractor != null && hoveringInteractor.gameObject.CompareTag("LeftController"))
        {
            TriggerOrGrabEvent.Invoke();
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if (args.interactorObject is XRBaseControllerInteractor controller)
        {
            hoveringInteractor = controller;
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        if (args.interactorObject == hoveringInteractor)
        {
            hoveringInteractor = null;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        TriggerOrGrabEvent.Invoke();
    }
}
