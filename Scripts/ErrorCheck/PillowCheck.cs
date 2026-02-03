using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowCheck : MonoBehaviour, ReturnedInterface
{
    public Container shelfParent;

    public bool isReturned()
    {
        return shelfParent.Contains(gameObject.transform);
    }

    public string getReturnedLabel()
    {
        return "Returning Pillow to shelf";
    }
}
