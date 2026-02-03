using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedSheetStatus : MonoBehaviour, CheckerInterface, ReturnedInterface
{
    public GameObject OpenBedSheet;
    public Container LaundryHamper;
    public bool isCorrect()
    {
        return OpenBedSheet.activeSelf;
    }
    public string getLabel()
    {
        return "Bed Sheet Placement";
    }
    public bool isReturned()
    {
        if (LaundryHamper == null || OpenBedSheet == null )
        {
            Debug.LogWarning("Missing references or colliders in BedSheetStatus");
            return false;
        }

        return !isCorrect() && LaundryHamper.Contains(transform);
    }

    public string getReturnedLabel()
    {
        return "Bed Sheet Disposal";
    }
}
