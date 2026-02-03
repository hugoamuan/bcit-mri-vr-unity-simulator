using UnityEngine;
using TMPro; // Required for TextMeshPro UI
using System.Collections.Generic;
// using TMPro.Examples;

public class PatientWalkManager : MonoBehaviour
{
    public GameObject walkingPatient = null;
    public GameObject seatedPatient = null;
    public Animator patientAnimator = null;
    public GameObject callInMenu = null;
    public GameObject patientPositionMenu = null;

    public ErrorCheck FirstErrorCheckHolder = null;
    public float speed = 0.75f;
    private bool isWalking = false;
    private bool isTurning = false;
    private Vector3 startPos = new Vector3(7.5f, 0f, -1.9f);
    private Vector3 currPos = new Vector3(7.5f, 0f, -1.9f);
    private Vector3 nextPos = new Vector3(7.5f, 0f, -1.9f);

    void Update()
    {
        if (isWalking)
        {
            // Current x val on curve z = 0.0006x^4, reversed because x moves in negative direction
            float absX = startPos.x - currPos.x; // 

            // Using derivative of curve to find change in z
            float deltaZ = 0.0024f * Mathf.Pow(absX, 3);

            // Get hypotenuse of triangle of rise / run
            float hyp = Mathf.Sqrt(Mathf.Pow(deltaZ, 2) + 1);
            // Scale triangle to hypotenuse of 1
            nextPos.x = currPos.x - 1.0f / hyp; // x movement negative
            nextPos.z = currPos.z + deltaZ / hyp;

            // Set patient rotation along curve
            float angle = Mathf.Atan(deltaZ) * Mathf.Rad2Deg;
            walkingPatient.transform.eulerAngles = new Vector3
            (
                walkingPatient.transform.eulerAngles.x,
                angle,
                walkingPatient.transform.eulerAngles.z
            );
            
            // Move to next position
            walkingPatient.transform.position = Vector3.Lerp(currPos, nextPos, speed * Time.deltaTime);

            currPos = walkingPatient.transform.position;

            if (currPos.x <= 0.8f)
            {
                isWalking = false;
                isTurning = true;
            }
        }

        if (isTurning)
        {
            walkingPatient.transform.Rotate(0f, -100f * speed * Time.deltaTime, 0f, Space.Self);
            if (walkingPatient.transform.eulerAngles.y % 360 >= 180 && walkingPatient.transform.eulerAngles.y % 360 <= 270)
            {
                isTurning = false;
                SeatPatient();
            }
        }
    }

    public void CallPatientIn()
    {
        FirstErrorCheckHolder.Check(OnContinueClick, () => {});
    }

    public void OnContinueClick()
    {
        patientAnimator.Play("WalkCycle", 0, 0.0f);
        isWalking = true;
        callInMenu.SetActive(false);
    }

    public void SeatPatient()
    {
        walkingPatient.SetActive(false);
        patientPositionMenu.SetActive(true);
    }
}