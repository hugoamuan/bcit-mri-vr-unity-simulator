using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowStatus : MonoBehaviour, CheckerInterface, ReturnedInterface
{
    public GameObject Pillow;
    public bool isCorrect()
    {
        return transform.childCount != 0;
    }
    public bool isReturned()
    {
        return !isCorrect();
    }
    public string getLabel()
    {
        return "Pillow Placement";
    }
    public string getReturnedLabel()
    {
        return "Pillow Clean Up";
    }

    public void Flip()
    {
        gameObject.transform.localPosition = new Vector3
        (
            gameObject.transform.localPosition.x,
            -6f,
            gameObject.transform.localPosition.z
        );
    }
}
