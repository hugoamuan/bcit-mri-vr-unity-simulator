using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class PatientStateManager : MonoBehaviour, CheckerInterface
{
    public GameObject patient;
    public PatientMenu patientMenu;
    public PatientMover patientMover;
    public DataBanker dataBanker;
    public DialogueController dialogueController;
    public List<PatientState> patientStates;
    public int initialState = 0;

    private GameObject patientModel;
    private Animator patientAnimator;
    private PatientState currentState;
    private bool isFlipped = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get patient model and animator
        patientModel = patient.transform.GetChild(0).gameObject;
        patientAnimator = patientModel.GetComponent<Animator>();

        // Check if states exist
        if (patientStates.Count <= 0)
        {
            throw new System.Exception("Could not set initial patient state: No patient states have been defined.");
        }

        if (initialState < 0 || initialState >= patientStates.Count) 
        {
            throw new System.Exception("Could not set initial patient state: Initial patient state is out of range.");
        }

        // Set initial patient position
        ChangePatientState(patientStates[initialState]);
    }

    public void ChangePatientState(string newStateLabel)
    {
        PatientState newState = null;
        foreach (PatientState ps in patientStates)
        {
            if (ps.label.Equals(newStateLabel))
            {
                newState = ps;
            }
        }
        if (newState != null)
        {
            //if (newState.conditions != null && newState.conditions.Count > 0) {
            foreach (GameObject conditionObject in newState.conditionObjects)
            {
                IPatientStateCondition checker = conditionObject.GetComponent<IPatientStateCondition>();
                if (checker == null)
                {
                    Debug.LogWarning($"{conditionObject.name} does not implement IPatientStateCondition.");
                    patientMenu.HideMenu();
                    continue;
                }

                if (!checker.IsStateChangeAllowed())
                {
                    dialogueController.InitiateDialogue("", "", checker.GetRefusalMessage(), 2);
                    Debug.Log($"{conditionObject.name} prevented patient state change");
                    patientMenu.HideMenu();
                    return;
                }
            }

            StartCoroutine(ChangePatientStateCoroutine(newState));
        }
        else
        {
            throw new System.Exception($"Could not change patient state: no match found for state '{newStateLabel}'");
        }
    }

    public void ChangePatientState(PatientState newState)
    {
        foreach (GameObject conditionObject in newState.conditionObjects)
        {
            IPatientStateCondition checker = conditionObject.GetComponent<IPatientStateCondition>();
            if (checker == null)
            {
                Debug.LogWarning($"{conditionObject.name} does not implement IPatientStateCondition.");
                patientMenu.HideMenu();
                continue;
            }

            if (!checker.IsStateChangeAllowed())
            {
                Debug.Log($"{conditionObject.name} prevented patient state change");
                patientMenu.HideMenu();
                return;
            }
        }
        StartCoroutine(ChangePatientStateCoroutine(newState));
    }

    private IEnumerator ChangePatientStateCoroutine(PatientState newState)
    {
        patientMenu.Disable();
        patientAnimator.enabled = false;
        currentState = null;

        newState.transition.additionalEvents?.Invoke();

        if (newState.options.flipTransition || (!isFlipped && newState.options.flipModel))
        {
            patient.transform.localScale = new Vector3
                (
                    -patient.transform.localScale.x,
                    patient.transform.localScale.y,
                    patient.transform.localScale.z
                );
            isFlipped = true;
        }

        if (newState.options.flipTransition || (!isFlipped || newState.options.flipModel))
            UpdateTransform(newState, newState.options.flipTransition);

        if (newState.transition.movementLabel != null && newState.transition.movementLabel != "") 
        {
            patientMover.SetCurrentMovement(newState.transition.movementLabel, ChangePatientState);
            patientMover.StartMoving();
        }

        if (newState.transition.animationName != null)
        {
            yield return StartCoroutine(PlayAnimation(newState.transition.animationName));
        }

        if (newState.options.flipTransition || (isFlipped && !newState.options.flipModel))
        {
            patient.transform.localScale = new Vector3
                (
                    -patient.transform.localScale.x,
                    patient.transform.localScale.y,
                    patient.transform.localScale.z
                );
            isFlipped = false;
            UpdateTransform(newState);
        }

        currentState = newState;

        patientMenu.SetItems(newState.menuItems, ChangePatientState, dataBanker.GetExamType());
        patientMenu.Enable();

        if (newState.options.immediateNextState != null && newState.options.immediateNextState != "")
        {
            yield return new WaitForSeconds(newState.options.nextStateDelay);
            try
            {
                ChangePatientState(newState.options.immediateNextState);
            }
            catch (System.Exception e)
            {
                Debug.Log($"Attempt to set patient state to {newState.options.immediateNextState} failed: {e}");
            }
        }
        
    }

    private void UpdateTransform(PatientState newState, bool offsetPosition = false)
    {
        if (newState.options.updateParent)
        {
            if (newState.transition.parent != null)
            {
                patient.transform.SetParent(newState.transition.parent.transform);
            }
            else
            {
                patient.transform.SetParent(null);
            }

        }
        if (newState.options.updateRotation)
        {
            patient.transform.rotation = Quaternion.Euler(0, newState.options.pivotDegrees, 0);
        }
        if (newState.options.updatePosition)
        {
            if (offsetPosition)
            {
                patient.transform.localPosition = new Vector3(
                    newState.options.xPosition,
                    newState.options.yPosition + newState.options.transitionOffsetY,
                    newState.options.zPosition
                );
            }
            else
            {
                patient.transform.localPosition = new Vector3(
                    newState.options.xPosition,
                    newState.options.yPosition,
                    newState.options.zPosition
                );
            }
        }
    }

    private IEnumerator PlayAnimation(string animationName)
    {
        if (!patientAnimator.HasState(0, Animator.StringToHash(animationName)))
        {
            yield break;
        }
        patientAnimator.enabled = true;
        patientAnimator.Play(animationName, 0, 0f);
        patientAnimator.speed = 1;

        yield return null;

        AnimatorStateInfo currentState = patientAnimator.GetCurrentAnimatorStateInfo(0);

        // Ensure animation has started
        yield return new WaitUntil(() => patientAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0);
        
        //int lastStateHash = currentState.fullPathHash;

        while (true)
        {
            if (currentState.loop) break;

            currentState = patientAnimator.GetCurrentAnimatorStateInfo(0);

            //// If there is another animation state, update to that one
            //if (currentState.fullPathHash != lastStateHash)
            //{
            //    lastStateHash = currentState.fullPathHash;
            //}

            if (!currentState.loop && currentState.normalizedTime >= 1f) break;

            yield return null;
        }
    }

    public PatientState GetCurrentState()
    {
        return currentState;
    }

    public bool isCorrect()
    {
        if (dataBanker.GetExamType() == "Knee")
            return currentState.label == "kneeLowered";
        else if (dataBanker.GetExamType() == "Head")
            return currentState.label == "lyingHeadToScanner";
        return false;
    }

    public string getLabel()
    {
        return "Patient position";
    }
}

[System.Serializable]
public class PatientState
{
    public string label;
    public List<string> menuItems;
    public List<GameObject> conditionObjects;
    public StateBeginTransition transition;
    public StateOptions options;
}

[System.Serializable]
public class StateBeginTransition
{
    public string? animationName;
    public GameObject? parent;
    public string? movementLabel;
    public UnityEvent additionalEvents;
}

[System.Serializable]
public class StateOptions
{
    public bool flipModel = false;
    public bool flipTransition = false;
    public bool updateParent = false;
    public bool updateRotation = false;
    public float pivotDegrees;
    public bool updatePosition = false;
    [Tooltip("New local X position")]
    public float xPosition;
    [Tooltip("New local Y position")]
    public float yPosition;
    [Tooltip("New local Z position")]
    public float zPosition;
    public float transitionOffsetY;
    public string immediateNextState;
    public float nextStateDelay = 0;
}
