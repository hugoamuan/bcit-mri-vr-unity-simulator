using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public Collider selfCollider;

    private void Awake()
    {
        if (selfCollider == null || !selfCollider.isTrigger)
        {
            Debug.LogWarning("Needs a trigger collider.");
        }
    }

    public bool Contains(Transform objTransform)
    {
        if (selfCollider == null) return false;

        // You can adjust this to use other criteria (center, bounds, etc.)
        return selfCollider.bounds.Contains(objTransform.position);
    }
}
