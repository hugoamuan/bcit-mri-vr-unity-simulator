using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientStateChangeCollider : MonoBehaviour
{
    public float triggerDistance = 0.2f; // Maximum distance for triggering state change
    public PatientStateManager patient;
    public GameObject expectedObject; // The specific object that can trigger state change
    public string startingStateLabel;
    public string targetStateLabel;

    private bool tryTrigger = false;

    // Update is called once per frame
    void Update()
    {
        if (tryTrigger)
        {
            PatientState currentState = patient.GetCurrentState();
            if (currentState == null) return;
            else if (currentState.label == startingStateLabel)
            {
                patient.ChangePatientState(targetStateLabel);
            }
            else
            {
                tryTrigger = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(expectedObject.transform.position, transform.position) < 5)
        {
            PatientState currentPatientState = patient.GetCurrentState();
            if (currentPatientState != null && currentPatientState.label != targetStateLabel)
            {
                tryTrigger = true;
            }
        }
        //if (other.gameObject == expectedObject) // Compare by reference
        //{
        //    Debug.Log("expected");
        //    PatientState currentPatientState = patient.GetCurrentState();
        //    if (currentPatientState != null && currentPatientState.label != targetStateLabel)
        //    {
        //        patient.ChangePatientState(targetStateLabel);
        //    }
        //    //TrySnapObject(other.transform);
        //}
    }
}
