using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedSmudger : MonoBehaviour
{
    public GameObject TissueObject;
    public GameObject PatientBed;
    public void SmudgeBed()
    {
        StartCoroutine(DelayedSmudge());
    }

    private IEnumerator DelayedSmudge()
    {
        yield return new WaitForSeconds(0.5f); // Wait 0.5 seconds

        if (gameObject.GetComponent<BedSheetStatus>().isReturned())
        {
            TissueObject.GetComponent<Tissue>().ApplySmudge(PatientBed);
        }
    }
}
