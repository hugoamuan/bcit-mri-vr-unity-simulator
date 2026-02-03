using UnityEngine;

/// <summary>
/// The TrayController class controls the movement of a tray object within defined Y boundaries.
/// It supports separate speeds for Y movement, includes methods for specific tray behaviors,
/// and prevents interference during special moves.
/// </summary>
public class TrayController : MonoBehaviour
{
    public float minY = 0.0f;
    public float maxY = -1.0f;
    private float ySpeed = 1.0f;
    public TableController tableController;
    public float FixedDistance = 0;
    private float targetY;
    private bool isMovingHome = false;
    private bool isMovingToFixedDistance = false;
    private bool IsSpecialMovementActive => isMovingHome || isMovingToFixedDistance;

    void Start()
    {
        targetY = transform.localPosition.y;
        ySpeed = tableController.speed*10;
    }

    public void MoveIn()
    {
        if (IsSpecialMovementActive) return;
        targetY = Mathf.Min(targetY + ySpeed * Time.deltaTime, maxY);

    }

    public void MoveOut()
    {
        if (IsSpecialMovementActive) return;
        targetY = Mathf.Max(targetY - ySpeed * Time.deltaTime, minY);
    }

    public void MoveToHome()
    {
        targetY = minY;
        isMovingHome = true;
        isMovingToFixedDistance = false;
    }

    public bool IsAtMin()
    {
        return Mathf.Approximately(transform.localPosition.y, minY);
    }

    public void MoveToFixedDistance()
    {
        targetY = Mathf.Min(targetY + FixedDistance, maxY);
        isMovingToFixedDistance = true;
        isMovingHome = false;
    }

    void Update()
    {
        Vector3 currentPos = transform.localPosition;

        // Use doubled Y speed during special moves
        float currentYSpeed = (isMovingHome || isMovingToFixedDistance) ? ySpeed * 2f : ySpeed;
        float newY = Mathf.MoveTowards(currentPos.y, targetY, currentYSpeed * Time.deltaTime);

        // Stop special move flags when targets are reached
        if (isMovingHome && Mathf.Approximately(newY, targetY))
        {
            isMovingHome = false;
        }

        if (isMovingToFixedDistance && Mathf.Approximately(newY, targetY))
        {
            isMovingToFixedDistance = false;
        }
        // Apply updated Y position
        transform.localPosition = new Vector3(currentPos.x, newY, currentPos.z);
    }
}
