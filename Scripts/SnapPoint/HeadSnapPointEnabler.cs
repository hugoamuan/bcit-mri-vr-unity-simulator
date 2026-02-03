using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSnapPointEnabler : MonoBehaviour
{
    public GameObject ToBeRemoved; // The headpiece to be removed
    public GameObject SnapPointToBeChecked; // The snap point to check
    public GameObject[] SnapPointsToBeToggled; // The snap points to be enabled/disabled

    private void Start()
    {
        if (SnapPointsToBeToggled != null)
        {
            foreach (GameObject snapPoint in SnapPointsToBeToggled)
            {
                snapPoint.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (IsChildOf(ToBeRemoved.transform, SnapPointToBeChecked.transform))
        {
            foreach (GameObject snapPoint in SnapPointsToBeToggled)
            {
                snapPoint.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject snapPoint in SnapPointsToBeToggled)
            {
                snapPoint.SetActive(true);
            }
        }
    }

    private bool IsChildOf(Transform child, Transform potentialParent)
    {
        foreach (Transform t in potentialParent)
        {
            if (t == child)
                return true;
        }
        return false;
    }
}
