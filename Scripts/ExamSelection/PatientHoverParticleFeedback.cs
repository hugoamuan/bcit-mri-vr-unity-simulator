using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class PatientHoverParticleFeedback : XRBaseInteractable
{
    public PatientStateManager patient;
    public string examSelectionPatientStateLabel;
    public DataBanker databanker;
    public GameObject PillowSnapPoint;
    public string examType;
    public string bodyPartName;
    public DialogueController dialogueController;
    public ParticleSystem hoverParticles;
    public ParticleSystem selectionParticles;
    public InputActionReference rightActivateAction;
    public InputActionReference leftActivateAction;

    private int hoverCount = 0;
    private bool isHovered = false;
    private bool hasStarted = false;
    private bool selectingExam = false;

    void Start()
    {
        if (rightActivateAction != null)
        {
            rightActivateAction.action.Enable();
        }

        if (leftActivateAction != null)
        {
            leftActivateAction.action.Enable();
        }
    }


    void Update()
    {
        if (!hasStarted)
        {
            PatientState patientState = patient.GetCurrentState();
            if (patientState == null || patientState.label != examSelectionPatientStateLabel) return;

            hoverParticles.Emit(1);
            dialogueController.InitiateDialogue("Thank you for confirming. Could you confirm that we will be scanning your {0} today?", "", "", 0);
            hasStarted = true;
        }
    }


    void OnDisable()
    {
        if (rightActivateAction != null)
            rightActivateAction.action.performed -= OnActivatePressed;

        if (leftActivateAction != null)
            leftActivateAction.action.performed -= OnActivatePressed;
    }


    private void OnActivatePressed(InputAction.CallbackContext context)
    {
        PatientState patientState = patient.GetCurrentState();
        if (patientState == null || patientState.label != examSelectionPatientStateLabel) return;

        if (isHovered)
        {
            databanker.SetExamType(examType);
            if (databanker.GetExamType().ToLower().Contains("knee")) PillowSnapPoint.GetComponent<PillowStatus>().Flip();
            patient.ChangePatientState("standingExamSelected");
            if (selectionParticles != null)
                selectionParticles.Play();
            dialogueController.InitiateDialogue("Thank you for confirming. Could you confirm that we will be scanning your {0} today?", bodyPartName, "Yes, that's right", 1);
            var allInstances = FindObjectsOfType<PatientHoverParticleFeedback>();
            foreach (var instance in allInstances)
            {
                instance.gameObject.SetActive(false);
            }
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        isHovered = true;
        HandleHoverEntered();

        if (rightActivateAction != null)
            rightActivateAction.action.performed += OnActivatePressed;
        if (leftActivateAction != null)
            leftActivateAction.action.performed += OnActivatePressed;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        isHovered = false;
        HandleHoverExited();

        if (rightActivateAction != null)
            rightActivateAction.action.performed -= OnActivatePressed;
        if (leftActivateAction != null)
            leftActivateAction.action.performed -= OnActivatePressed;
    }

    private void HandleHoverEntered()
    {
        PatientState patientState = patient.GetCurrentState();
        if (patientState == null || patientState.label != examSelectionPatientStateLabel) return;

        dialogueController.SetUserText(bodyPartName);

        hoverCount++;
        if (hoverParticles != null && !hoverParticles.isPlaying)
            hoverParticles.Play();
    }

    private void HandleHoverExited()
    {
        hoverCount = Mathf.Max(hoverCount - 1, 0);
        if (hoverCount == 0 && hoverParticles != null && hoverParticles.isPlaying)
            hoverParticles.Stop();

        PatientState patientState = patient.GetCurrentState();
        if (patientState == null || patientState.label != examSelectionPatientStateLabel) return;

        dialogueController.SetUserText();
    }
}
