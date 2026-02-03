using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilStatus : MonoBehaviour, CheckerInterface, ReturnedInterface
{
    private List<GameObject> examTypes = new List<GameObject>();
    public List<GameObject> coils = new List<GameObject>();
    public DataBanker dataBanker;
    public Container shelfParent;
    void Start()
    {
        foreach (Transform child in transform)
        {
            examTypes.Add(child.gameObject);
        }
    }

    public bool isCorrect() {
        foreach (GameObject examType in examTypes)
        {
            if (dataBanker.GetExamType().Contains(examType.name))
            {
                int numParts = GetNumParts(examType);
                int checkNumParts = 0;

                foreach (Transform childCoil in examType.transform)
                {
                    foreach (Transform realCoil in childCoil.gameObject.transform)
                    {
                        if (realCoil.gameObject.CompareTag("Coil"))
                        {
                            checkNumParts++;
                        }
                    }
                }

                if (checkNumParts == numParts) return true;
            }
        }
        return false;
    }

    private int GetNumParts(GameObject exam)
    {
        int numParts = 0;
        foreach (Transform child in exam.transform)
        {
            if (child.gameObject.name.ToLower().Contains("snappoint")) numParts++;
        }
        return numParts;
    }

    public bool isReturned()
    {
        foreach (GameObject coil in coils)
        {
            if (coil == null) continue; // Skip if the coil is null
            if (!shelfParent.Contains(coil.transform)) return false; // If the coil has children, it's not returned
        }
        return true;
    }

    public string getLabel() {
        return "Coil Selection";
    }

    public string getReturnedLabel() {
        return "Returning Coils to Shelf";
    }
}
