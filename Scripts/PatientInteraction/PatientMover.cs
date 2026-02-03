using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientMover : MonoBehaviour
{
    public string walkAnimation = "m35a@WalkCycle";
    public Animator patientAnimator;
    public List<PatientMovement> allPatientMovements;
    private PatientMovement currentMovement;
    private float speed = 1;
    private BezierSpline? currentSpline = null;
    private float speedFactor;
    private float targetStepSize;
    private float lastStepSize;
    private float t;
    private bool isMoving = false;

    private void Start()
    {
        speedFactor = speed / 100; // approximate value to start
        targetStepSize = 0;
        t = speedFactor / 5;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Check if movement exists
            if (currentSpline == null)
            {
                isMoving = false;
                throw new System.Exception("Could not move patient: no patient movement has been set.");
            }

            // Take a note of your previous position.
            Vector3 previousPosition = transform.position;

            // Advance on the curve to the next t;
            transform.position = currentSpline.GetPoint(t);

            if (transform.position == previousPosition)
            {
                isMoving = false;
                currentMovement.onMovementEndCallback(currentMovement.nextPatientStateLabel);
                return;
            }

            targetStepSize = speed * Time.deltaTime; // calculate target distance from last frame

            // Measure your movement length
            lastStepSize = Vector3.Magnitude(transform.position - previousPosition);

            // Accelerate or decelerate according to your latest step size.
            if (lastStepSize < targetStepSize)
            {
                speedFactor *= 1.1f;
            }
            else
            {
                speedFactor *= 0.9f;
            }

            transform.LookAt(transform.position + currentSpline.GetDirection(t));
            t += speedFactor * Time.deltaTime;
        }
    }

    public void SetCurrentMovement(PatientMovement movement)
    {
        currentMovement = movement;
        SetCurrentSpline(currentMovement);
        SetSpeed(currentMovement);
    }

    public void SetCurrentMovement(string movementLabel, PatientMovement.callback movementEndCallback)
    {
        foreach (PatientMovement movement in allPatientMovements)
        {
            if (movement.label == movementLabel)
            {
                currentMovement = movement;
                SetCurrentSpline(currentMovement);
                SetSpeed(currentMovement);
                movement.onMovementEndCallback = movementEndCallback;
                return;
            }
        }
        throw new System.Exception($"Could not set patient movement: {movementLabel} is not a valid movement name");
    }

    public void SetCurrentSpline(BezierSpline spline)
    {
        currentSpline = spline;
    }

    public void SetCurrentSpline(PatientMovement movement)
    {
        currentSpline = movement.pathSpline;
    }

    public void SetSpeed(PatientMovement movement)
    {
        speedFactor = speed / 100; // approximate value to start
        targetStepSize = 0;
        t = speedFactor / 5;
        this.speed = movement.speed;
    }

    public void StartMoving()
    {
        if (currentSpline == null)
        {
            isMoving = false;
            throw new System.Exception("Could not move patient: no patient movement has been set.");
        }

        isMoving = true;

        // Play walk animation if defined
        if (!string.IsNullOrEmpty(walkAnimation))
        {
            patientAnimator.enabled = true;
            patientAnimator.Play(walkAnimation, 0, 0f);
            patientAnimator.speed = 1;
        }

        t = 0f;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    //public void StartMoving(string movementLabel)
    //{
    //    SetCurrentMovement(movementLabel);
    //    isMoving = true;
    //}
}

[System.Serializable]
public class PatientMovement
{
    public string label;
    public BezierSpline pathSpline;
    public float speed;
    public string nextPatientStateLabel;
    [HideInInspector] public delegate void callback(string label);
    [HideInInspector] public callback onMovementEndCallback;
}
