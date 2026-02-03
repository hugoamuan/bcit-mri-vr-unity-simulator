using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabTracker : MonoBehaviour
{
    public PhysicsCablePlus cable;
    public bool isGrabObject;

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("GrabTracker requires XRGrabInteractable on the same GameObject.");
        }
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (cable != null)
        {
            if (isGrabObject)
                cable.SetGrabObjectHeld(true);
            else
                cable.SetLargeObjectHeld(true);
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (cable != null)
        {
            if (isGrabObject)
                cable.SetGrabObjectHeld(false);
            else
                cable.SetLargeObjectHeld(false);
        }
    }

}
