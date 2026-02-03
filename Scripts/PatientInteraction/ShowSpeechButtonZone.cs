using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowSpeechButtonZone: MonoBehaviour
{
    public Transform cameraTransform;
    public XRRayInteractor leftRayInteractor;
    public XRRayInteractor rightRayInteractor;

    public GameObject speechButton; 
    public float rayAngleThreshold = 30f;
    public float buttonDropHeight = 0.2f;
    public float animationDuration = 0.5f;

    private bool isAnimating = false;
    private bool isShowing = false;
    private Vector3 shownPosition;
    private Vector3 hiddenPosition;

    void Start()
    {
        shownPosition = speechButton.transform.localPosition;
        hiddenPosition = shownPosition + Vector3.up * buttonDropHeight;
        speechButton.transform.localPosition = hiddenPosition;
    }

    void Update()
    {
        bool leftInZone = CheckRayAboveView(leftRayInteractor);
        bool rightInZone = CheckRayAboveView(rightRayInteractor);

        bool currentlyInZone = leftInZone || rightInZone;

        if (currentlyInZone && !isShowing && !isAnimating)
        {
            ShowButton();
        }
        else if (!currentlyInZone && isShowing && !isAnimating)
        {
            HideButton();
        }

        //isTriggered = currentlyInZone;
    }

    bool CheckRayAboveView(XRRayInteractor ray)
    {
        // Camera's current view direction
        Vector3 cameraForward = cameraTransform.forward;

        // Ray's direction
        Vector3 rayDirection;
        if (ray.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            rayDirection = (hit.point - ray.transform.position).normalized;
        }
        else
        {
            // Fallback: use ray interactor's forward
            rayDirection = ray.transform.forward;
        }

        // Compare vertical (pitch) angle difference between camera and ray
        float verticalAngleDifference = Vector3.SignedAngle(rayDirection, cameraForward, cameraTransform.right);


        if (isShowing) return verticalAngleDifference > (rayAngleThreshold - 45);
        else return verticalAngleDifference > rayAngleThreshold;
    }

    void ShowButton()
    {
        isAnimating = true;
        isShowing = true;
        Vector3 start = speechButton.transform.localPosition;
        Vector3 end = shownPosition;
        StartCoroutine(SlideToPosition(start, end));
    }

    void HideButton()
    {
        isAnimating = true;
        isShowing = false;
        Vector3 start = speechButton.transform.localPosition;
        Vector3 end = hiddenPosition;
        StartCoroutine(SlideToPosition(start, end));
    }

    System.Collections.IEnumerator SlideToPosition(Vector3 start, Vector3 end)
    {
        float t = 0;
        while (t < 1)
        {
            speechButton.transform.localPosition = Vector3.Lerp(start, end, t);
            t += Time.deltaTime / animationDuration;
            yield return null;
        }
        speechButton.transform.localPosition = end;
        isAnimating = false;
    }
}
