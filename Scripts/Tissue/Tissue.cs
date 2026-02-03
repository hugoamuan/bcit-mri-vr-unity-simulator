using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tissue : MonoBehaviour, ReturnedInterface
{
    public GameObject TissueObj;
    public List<GameObject> DirtyObjects = new List<GameObject>();

    void Update()
    {
        TissueObj.GetComponent<BoxCollider>().enabled = true;
    }

    public void OnGrab()
    {
        TissueObj.GetComponent<Collider>().isTrigger = true;
    }

    public void OnRelease()
    {
        TissueObj.GetComponent<Collider>().isTrigger = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("smudge"))
        {
            RevertSmudge(other.gameObject);
        }
    }

    public void RevertSmudge(GameObject smudge)
    {
        smudge.SetActive(false);
        // DirtyObjects.Remove(smudge.transform.parent.gameObject);
    }

    public void AddDirtyObject(GameObject obj)
    {
        if (!DirtyObjects.Contains(obj)) DirtyObjects.Add(obj);
    }

    public void ApplySmudge(GameObject customObj)
    {
        if (!DirtyObjects.Contains(customObj))
        {
            DirtyObjects.Add(customObj);
        }
        foreach (Transform smudge in customObj.transform)
        {
            if (smudge.gameObject.name.ToLower().Contains("smudge"))
            {
                smudge.gameObject.SetActive(true);
            }
        }
    }

    public void ApplySmudgeAll()
    {
        foreach (GameObject obj in DirtyObjects)
        {
            foreach (Transform smudge in obj.transform)
            {
                if (smudge.gameObject.name.ToLower().Contains("smudge"))
                {
                    smudge.gameObject.SetActive(true);
                }
            }
        }
    }

    //check if all smudge disabled instead of checking if it is removed from the list
    public bool isReturned()
    {
        foreach (GameObject obj in DirtyObjects)
        {
            foreach (Transform smudge in obj.transform)
            {
                if (smudge.gameObject.name.ToLower().Contains("smudge"))
                {
                    if (smudge.gameObject.activeSelf) return false;
                }
            }
        }
        return true;
    }

    public string getReturnedLabel()
    {
        return "Equipment Cleaning";
    }
}
