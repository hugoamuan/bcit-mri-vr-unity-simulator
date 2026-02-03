using UnityEngine;
/// <summary>
/// Simulates a cable connection between two objects using a LineRenderer.
/// The cable dynamically updates its shape each frame, applying a sagging effect 
/// based on a sine function. The number of segments determines the smoothness of the curve.
/// </summary>
public class DumbCable : MonoBehaviour
{
    public Transform grabObject;
    public Transform largeObject;
    public int numSegments = 10;
    public float sagAmount = 0.15f;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        if (lineRenderer == null)
        {
            Debug.LogError("No LineRenderer component found on the GameObject.");
            return;
        }

        lineRenderer.positionCount = numSegments + 1;
    }

    void Update()
    {
        if (lineRenderer == null) return;

        Vector3 grabPos = grabObject.position;
        Vector3 largePos = largeObject.position;

        for (int i = 0; i <= numSegments; i++)
        {
            float t = i / (float)numSegments;
            Vector3 position = Vector3.Lerp(grabPos, largePos, t);
            position.y -= Mathf.Sin(t * Mathf.PI) * sagAmount;
            lineRenderer.SetPosition(i, position);
        }
    }
}
