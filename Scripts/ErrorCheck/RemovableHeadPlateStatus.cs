using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovableHeadPlateStatus : MonoBehaviour, ReturnedInterface
{
    public bool isReturned()
    {
        return transform.childCount != 0;
    }

    public string getReturnedLabel()
    {
        return "Head Plate Replacement";
    }
}
