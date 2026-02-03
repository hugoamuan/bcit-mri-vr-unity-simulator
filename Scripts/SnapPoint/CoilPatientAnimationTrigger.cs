using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilPatientAnimationTrigger : MonoBehaviour, ISnappable
{
    public PatientStateManager patient;
    public string startingStateLabel;
    public string targetStateLabel;

    private bool tryTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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

    void ISnappable.OnSnapped(Transform snapPointParent)
    {
        tryTrigger = true;
    }
}
