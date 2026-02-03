using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientWalkIn : MonoBehaviour
{
    public GameObject RightCurtainController;
    private bool hasBeenTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("headtarget") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            RightCurtainController.GetComponent<SmoothCurtainAnimator>().TriggerAnimation();
        }
    }
}
