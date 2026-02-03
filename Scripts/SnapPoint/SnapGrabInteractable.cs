using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System.Collections.Generic;


[DefaultExecutionOrder(100)]
/// <summary>
/// A custom XRGrabInteractable that allows objects to be grabbed only 
/// if the user's controller is within a specified grab radius.
/// This script also maintains positional and rotational offsets 
/// while ensuring certain axes remain frozen if needed.
/// It also preserves the initial rotation when grabbed.
/// </summary>
public class SnapGrabInteractable : XRGrabInteractable
{
    [Header("Snap Settings")]
    public float positionOffset = 0.5f;

    [Header("Grab Settings")]
    public float grabRadius = 1.25f; // The radius within which the controller must be to grab

    private Rigidbody rb;
    private Transform controllerTransform;
    private new Collider[] colliders;
    private RigidbodyConstraints originalConstraints;
    private bool freezeRotationX, freezeRotationY, freezeRotationZ;
    private Quaternion initialRotationOffset; // Stores the rotation offset when grabbed
    private bool isGrabbed = false;
    public UnityEvent OnGrabbed = null; // Event to trigger when grabbed
    public UnityEvent OnReleased = null; // Event to trigger when released
    [Header("Release Settings")]
    [Tooltip("Delay in seconds before triggering OnReleased event.")]
    public float onReleasedDelay = 0f;

    private List<bool> colliderInitialStates = new List<bool>();
    private int originalLayer;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>(true);

        attachEaseInTime = 0f;
        trackPosition = false;
        trackRotation = false;
        originalLayer = gameObject.layer;
    }

    /// Allows grabbing if controller is close enough.
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (interactor.transform != null)
        {
            float distance = Vector3.Distance(interactor.transform.position, transform.position);
            return distance <= grabRadius && base.IsSelectableBy(interactor);
        }
        return false;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (OnGrabbed != null) OnGrabbed.Invoke(); // Trigger the OnGrabbed event if assigned
        gameObject.layer = LayerMask.NameToLayer("Default");
        base.OnSelectEntered(args);

        colliderInitialStates.Clear();

        Vector3 worldScale = transform.lossyScale;

        if (transform.parent != null)
            transform.SetParent(null, true); // Keep world position when unparenting

        controllerTransform = args.interactorObject.transform;
        isGrabbed = true; // Enable LateUpdate when grabbed

        if (rb != null)
        {
            originalConstraints = rb.constraints;
            freezeRotationX = (rb.constraints & RigidbodyConstraints.FreezeRotationX) != 0;
            freezeRotationY = (rb.constraints & RigidbodyConstraints.FreezeRotationY) != 0;
            freezeRotationZ = (rb.constraints & RigidbodyConstraints.FreezeRotationZ) != 0;

            rb.isKinematic = true; // Disable physics during grab
        }

        // Store the initial rotation offset when grabbed
        if (controllerTransform != null)
        {
            initialRotationOffset = Quaternion.Inverse(controllerTransform.rotation) * transform.rotation;
        }

        foreach (var col in colliders)
        {
            colliderInitialStates.Add(col.enabled);
            col.enabled = false;
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        isGrabbed = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = originalConstraints;
            rb.WakeUp();
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = colliderInitialStates[i];
        }

        controllerTransform = null;

        base.OnSelectExited(args);
        transform.SetParent(null, true);

        if (onReleasedDelay > 0f) {
            StartCoroutine(InvokeReleaseEventAfterDelay());
            gameObject.layer = originalLayer;
        }
        else if (OnReleased != null) {
            OnReleased.Invoke();
            gameObject.layer = originalLayer;
        }
    }
    private System.Collections.IEnumerator InvokeReleaseEventAfterDelay()
    {
        yield return new WaitForSeconds(onReleasedDelay);
        if (OnReleased != null) OnReleased.Invoke();
    }

    private void FixedUpdate()
    {
        if (!isGrabbed || controllerTransform == null) return;

        // Maintain positional offset
        Vector3 targetPosition = controllerTransform.position + controllerTransform.TransformDirection(new Vector3(0, 0, positionOffset));
        transform.position = targetPosition;

        if (rb == null) return;

        // Apply preserved rotation
        Quaternion targetRotation = controllerTransform.rotation * initialRotationOffset;
        Vector3 euler = targetRotation.eulerAngles;

        if (freezeRotationX) euler.x = transform.rotation.eulerAngles.x;
        if (freezeRotationY) euler.y = transform.rotation.eulerAngles.y;
        if (freezeRotationZ) euler.z = transform.rotation.eulerAngles.z;

        transform.rotation = Quaternion.Euler(euler);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
