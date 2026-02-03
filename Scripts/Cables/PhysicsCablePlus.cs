using UnityEngine;

public class PhysicsCablePlus : PhysicsCable
{
    [Header("Leash Settings")]
    [Tooltip("Maximum allowed distance between endpoints before pulling occurs.")]
    public float leashRange = 5f;

    [Tooltip("Force multiplier for pulling the ungrabbed object.")]
    public float leashPullForce = 50f;

    private bool isGrabObjectHeld = false;
    private bool isLargeObjectHeld = false;

    private Rigidbody unheldRb;
    private Transform held;
    private Transform unheld;

    public void SetGrabObjectHeld(bool value) => isGrabObjectHeld = value;
    public void SetLargeObjectHeld(bool value) => isLargeObjectHeld = value;

    void FixedUpdate()
    {
        if (grabObject == null || largeObject == null)
            return;

        float currentDistance = Vector3.Distance(grabObject.position, largeObject.position);
        bool onlyOneHeld = isGrabObjectHeld ^ isLargeObjectHeld;

        if (currentDistance > leashRange && onlyOneHeld)
        {
            held = isGrabObjectHeld ? grabObject : largeObject;
            unheld = isGrabObjectHeld ? largeObject : grabObject;

            if (unheldRb == null || unheldRb.gameObject != unheld.gameObject)
            {
                unheldRb = unheld.GetComponentInParent<Rigidbody>();
            }

            if (unheldRb != null && !unheldRb.isKinematic)
            {
                Vector3 pullDir = (held.position - unheld.position).normalized;
                float excessDistance = currentDistance - leashRange;

                Vector3 pullForce = pullDir * excessDistance * leashPullForce;

                unheldRb.AddForce(pullForce, ForceMode.Acceleration);
            }
        }
        else
        {
            unheldRb = null;
        }
    }

    // protected override void Update()
    // {
    //     // Keep visual cable update in Update as before
    //     base.Update();

    //     // Optional: Debug log for info (can be moved or removed)
    //     if (grabObject != null && largeObject != null)
    //     {
    //         float dist = Vector3.Distance(grabObject.position, largeObject.position);
    //         Debug.Log($"[CablePlus] Distance: {dist:F2}, LeashRange: {leashRange}, OnlyOneHeld: {isGrabObjectHeld ^ isLargeObjectHeld}");
    //     }
    // }
}
