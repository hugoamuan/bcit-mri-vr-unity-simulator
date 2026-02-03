using UnityEngine;

public class SmoothCurtainAnimator : MonoBehaviour
{
    [Header("Target Values (for UnityEvent)")]
    public Vector3 targetPosition;
    public Vector3 targetScale = Vector3.one;

    public float moveSpeed = 2f;
    public float scaleSpeed = 2f;

    private bool animate = false;
    private bool goingToHome = false;

    private Vector3 homePosition;
    private Vector3 homeScale;

    void Start()
    {
        homePosition = transform.localPosition;
        homeScale = transform.localScale;
        TriggerAnimation();  // Open the curtain at the start
    }

    void Update()
    {
        if (!animate) return;

        if (goingToHome)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);

            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f &&
                Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                animate = false;
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, homePosition, Time.deltaTime * moveSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, homeScale, Time.deltaTime * scaleSpeed);

            if (Vector3.Distance(transform.localPosition, homePosition) < 0.01f &&
                Vector3.Distance(transform.localScale, homeScale) < 0.01f)
            {
                animate = false;
            }
        }
    }

    public void TriggerAnimation()
    {
        if (animate) return;

        goingToHome = !goingToHome;  // Toggle direction
        animate = true;
    }
}
