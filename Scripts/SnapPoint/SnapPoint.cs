using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// The SnapPoint class allows a specific object to snap to a defined position and rotation when it enters a trigger area.
/// Instead of checking tags, it directly compares the colliding GameObject with the expected reference.
/// </summary>
public class SnapPoint : MonoBehaviour
{
    public float snapDistance = 0.2f; // Maximum distance for snapping
    public Vector3 snapPosition; // Position to snap to
    public Vector3 snapRotation; // Rotation to snap to
    public GameObject expectedObject; // The specific object that can snap
    public GameObject snapConditionObject; // Optional object containing ISnapCondition component
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("SnapPoints");
    }
    private void OnTriggerStay(Collider other)
    {
        // Allow either exact expectedObject match, or if expectedObject is null accept any "Coil"-tagged object.
        // Also handle a common misconfiguration: if the user accidentally set the `expectedObject` to
        // the condition object (it implements ISnapCondition) and didn't assign `snapConditionObject`,
        // auto-assign it and clear expectedObject so snapping uses tag-based matching.

        // Auto-fix misconfigured inspector: expectedObject used as condition
        if (expectedObject != null && snapConditionObject == null)
        {
            ISnapCondition maybeCondition = expectedObject.GetComponent<ISnapCondition>();
            if (maybeCondition != null)
            {
                snapConditionObject = expectedObject;
                Debug.Log($"[SnapPoint] Auto-assigned '{expectedObject.name}' as Snap Condition for '{gameObject.name}' because it implements ISnapCondition.\nPlease set the SnapPoint.Expected Object to the coil GameObject (or leave empty to accept any object tagged 'Coil').");
                // Clear expectedObject so we fall back to tag-based detection
                expectedObject = null;
            }
        }

        bool candidateMatches = false;
        if (expectedObject == null)
        {
            // Fall back to tag-based matching for coils when no specific expectedObject is set
            candidateMatches = other.gameObject.CompareTag("Coil");
        }
        else
        {
            candidateMatches = other.gameObject == expectedObject; // exact reference match
        }

        if (candidateMatches)
        {
            TrySnapObject(other.transform);
        }
    }

    private void TrySnapObject(Transform obj)
    {
        // Check snap conditions before allowing snap
        if (!CheckSnapConditions())
        {
            return; // Don't snap if conditions are not met
        }
        float distance = Vector3.Distance(obj.position, transform.position);

        if (distance <= snapDistance)
        {
            SnapObject(obj);
        }
    }

    /// <summary>
    /// Checks all snap conditions to determine if snapping is allowed.
    /// </summary>
    /// <returns>True if all conditions are met or no conditions exist, false otherwise.</returns>
    public bool CheckSnapConditions()
    {
        // If no condition object is assigned, allow snapping
        if (snapConditionObject == null)
        {
            return true;
        }

        // Check if the condition object has an ISnapCondition component
        ISnapCondition snapCondition = snapConditionObject.GetComponent<ISnapCondition>();
        
        if (snapCondition == null)
        {
            Debug.LogWarning($"SnapConditionObject assigned but does not implement ISnapCondition interface on '{snapConditionObject.name}'");
            return true; // Default to allowing snap if misconfigured
        }

        // Check if snap is allowed
        if (!snapCondition.IsSnapAllowed())
        {
            // log refusal message
            Debug.Log($"Snap prevented: {snapCondition.GetRefusalMessage()}");
            return false;
        }

        return true;
    }

    private void SnapObject(Transform obj)
    {
        // Disable gravity if the object has a Rigidbody
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Attempt to set the parent
        obj.SetParent(transform);

        obj.gameObject.layer = LayerMask.NameToLayer("SnappedObjects");

        // Set position using custom snap position values
        obj.localPosition = snapPosition;

        // Set rotation using custom snap rotation values
        obj.localRotation = Quaternion.Euler(snapRotation);

        XRGrabInteractable grab = obj.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            // Make sure "SnappedObjects" is defined in Project Settings > XR Interaction Toolkit
            grab.interactionLayers = InteractionLayerMask.GetMask("SnappedObjects");
        }

        // Check if the object implements ISnappable and call OnSnapped()
        ISnappable snappable = obj.GetComponent<ISnappable>();
        if (snappable != null)
        {
            snappable.OnSnapped(transform.parent);
        }
        DisableGhost(obj);
    }

    private void DisableGhost(Transform obj)
    {
        if (!obj.gameObject.CompareTag("Coil")) return;
        
        GameObject parent = obj.parent.gameObject; 
        
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.CompareTag("Ghost"))
            {
                child.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}

// Interface for custom snap behavior
public interface ISnappable
{
    void OnSnapped(Transform snapPointParent);
}
