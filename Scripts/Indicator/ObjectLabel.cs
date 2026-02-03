using UnityEngine;
using TMPro;


/// <summary>
/// Creates a TextMeshPro label above an object that always faces the camera.
/// </summary>
public class ObjectLabel : MonoBehaviour
{
    [Header("Label Settings")]
    public string labelText;
    public float heightOffset = 0.2f;
    public Color textColor = Color.white;
    public float fontSize = 0.5f;

    private GameObject labelCanvas;
    private TextMeshPro labelTextComponent;

    void Start()
    {
        CreateLabel();
    }

    /// <summary>
    /// Creates a TextMeshPro label as a child object on the Labels layer.
    /// </summary>
    void CreateLabel()
    {
        GameObject textObject = new GameObject("LabelText");
        textObject.transform.SetParent(transform, false);
        
        textObject.layer = LayerMask.NameToLayer("Labels");

        labelTextComponent = textObject.AddComponent<TextMeshPro>();
        labelTextComponent.text = labelText;
        labelTextComponent.fontSize = fontSize;
        labelTextComponent.color = textColor;
        labelTextComponent.alignment = TextAlignmentOptions.Center;
        
        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(2, 1);

        labelCanvas = textObject;
    }

    /// <summary>
    /// Updates label position above the object and rotates it to face the camera.
    /// </summary>
    void LateUpdate()
    {
        if (labelCanvas != null && Camera.main != null)
        {
            Vector3 targetPosition = transform.position + Vector3.up * heightOffset;
            labelCanvas.transform.position = targetPosition;
            
            labelCanvas.transform.rotation = Camera.main.transform.rotation;
        }
    }
}