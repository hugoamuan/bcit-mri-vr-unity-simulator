using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToParent : MonoBehaviour
{
    private GameObject parentObject;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    void Start()
    {
        parentObject = transform.parent.gameObject;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void ReturnToParentObject()
    {
        transform.SetParent(parentObject.transform);
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}
