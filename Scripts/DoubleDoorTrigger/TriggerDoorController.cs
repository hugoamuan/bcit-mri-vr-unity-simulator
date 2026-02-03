using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Controller for the Zone 4 main door. Used to control the state and 
/// animations of the zone 4 door.
/// 
/// Implements the CheckerInterface to make it checkable as part of a feedback
/// report if the user has closed the door before progressing on the test.
/// 
/// Implements the IPatientStateCondition so the patient can refuse to go
/// through the door if it's closed.
/// </summary>
public class TriggerDoorController : MonoBehaviour, CheckerInterface, IPatientStateCondition
{
    [SerializeField] private Animator leftDoor = null;
    [SerializeField] private Animator rightDoor = null;
    private bool isOpen = false;

    public void OpenDoor()
    {
        if (leftDoor.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            //Transform parent = door.parent;
            if (leftDoor != null && rightDoor != null)
            {
                if (!isOpen)
                {
                    // Opening the door
                    leftDoor.Play("DoorOpenClockwise", 0, 0.0f);
                    rightDoor.Play("DoorOpenCounterclockwise", 0, 0.0f);

                    isOpen = true;
                }
                else
                {
                    // Closing the door
                    leftDoor.Play("DoorCloseCounterclockwise", 0, 0.0f);
                    rightDoor.Play("DoorCloseClockwise", 0, 0.0f);

                    isOpen = false;
                }
            }
        }
    }

    public string getLabel()
    {
        return "Zone 4 door closed";
    }

    public bool isCorrect()
    {
        return !isOpen;
    }

    public bool IsStateChangeAllowed()
    {
        return isOpen;
    }

    public string GetRefusalMessage()
    {
        return "The door is closed...";
    }
}
