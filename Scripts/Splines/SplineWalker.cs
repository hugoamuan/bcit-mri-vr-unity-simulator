using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class SplineWalker : MonoBehaviour
{
    public BezierSpline spline;
    public float speed;
    private float speedFactor;
    private float targetStepSize;
    private float lastStepSize;
    private float t;
    private bool isMoving = true;

    private void Start()
    {
        speedFactor = speed / 100; // approximate value to start
        targetStepSize = speed / 60f; // assuming 60 frames per second to start
        t = speedFactor;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Take a note of your previous position.
            Vector3 previousPosition = transform.position;

            // Advance on the curve to the next t;
            transform.position = spline.GetPoint(t);

            if (transform.position == previousPosition)
            {
                isMoving = false;
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

            transform.LookAt(transform.position + spline.GetDirection(t));
            t += speedFactor * Time.deltaTime;
        }
    }

    public void SetSpline(BezierSpline spline)
    {
        this.spline = spline;
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StartMoving(BezierSpline spline)
    {
        SetSpline(spline);
        isMoving = true;
    }
}