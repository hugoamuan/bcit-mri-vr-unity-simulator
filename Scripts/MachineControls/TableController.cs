using UnityEngine;
/// <summary>
/// This class controls the vertical movement of a table in a VR environment.
/// It allows the table to move between a specified minimum and maximum Y position at a defined speed.
/// The table can be moved up, down, or directly to its maximum position, and it will smoothly transition between positions when moved.
/// The class also provides methods to check if the table is at its minimum or maximum position.
/// </summary>
public class TableController : MonoBehaviour, CheckerInterface, IPatientStateCondition
{
    public float minY; // Minimum Y position
    public float maxY; // Maximum Y position
    public float speed; // Movement speed
    private float targetY; // The Y position the table is moving toward

    void Start()
    {
        targetY = transform.localPosition.y;
        // Debug.Log($"Table initialized at Y position: {targetY}");
    }
    /// <summary>
    /// Moves the table up by increasing the target Y position.
    /// Ensures it does not exceed the maxY limit.
    /// </summary>
    public void MoveUp()
    {
        float previousY = targetY;
        targetY = Mathf.Min(targetY + speed * Time.deltaTime, maxY);
        // Debug.Log($"MoveUp called. Y position changed from {previousY} to {targetY}");
    }
    /// <summary>
    /// Moves the table down by decreasing the target Y position.
    /// Ensures it does not go below the minY limit.
    /// </summary>
    public void MoveDown()
    {
        float previousY = targetY;
        targetY = Mathf.Max(targetY - speed * Time.deltaTime, minY);
        // Debug.Log($"MoveDown called. Y position changed from {previousY} to {targetY}");
    }

    // Instantly sets the target position to the maximum height.
    public void MoveToMax()
    {
        targetY = maxY;
        // Debug.Log("MoveToMax called. Table moving to max Y position.");
    }

    // Checks if the table is at its maximum height.
    public bool IsAtMax()
    {
        return Mathf.Approximately(transform.localPosition.y, maxY);
    }

    // Checks if the table is at its minimum height.
    public bool IsAtMin()
    {
        return Mathf.Approximately(transform.localPosition.y, minY);
    }

    // Smoothly moves the table toward the target Y position each frame.
    void Update()
    {
        if (!Mathf.Approximately(transform.localPosition.y, targetY))
        {
            float newY = Mathf.MoveTowards(transform.localPosition.y, targetY, speed * Time.deltaTime);
            // Debug.Log($"Updating table Y position: {transform.localPosition.y} → {newY}");
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
    }

    public bool isCorrect()
    {
        return IsAtMin();
    }
    public string getLabel()
    {
        return "Table Height";
    }

    public bool IsStateChangeAllowed()
    {
        return IsAtMin();
    }

    public string GetRefusalMessage()
    {
        return "The table is kind of high...";
    }
}
