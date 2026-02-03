using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to check the bottom coil status (correct or not) by the 
/// ErrorChecks.
/// Could not find which class contains those tests in Unity, but I know it's
/// related to the components ErrorChecks.
/// Currently only used when the user decides to do a head exam.
/// </summary>
public class BottomCoilStatus : MonoBehaviour, CheckerInterface
{
    /// <summary>
    /// Returns if a Coil object is attached to the component.
    /// (For some reason the SnapPoint in the MRI is the one doing the
    /// check.)
    /// </summary>
    public bool isCorrect()
    {
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
            if (child.CompareTag("Coil")) {
                return true;
            }
        }
        return false;
    }

    public string getLabel()
    {
        return "Coil Bottom Selection";
    }
}
