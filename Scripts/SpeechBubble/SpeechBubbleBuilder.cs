using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.Events; // Required for UnityEvent
using UnityEngine.InputSystem; // For keyboard and XR input

public class SpeechBubbleBuilder : MonoBehaviour
{
    public Transform headTransform;
    public Vector3 offset = new Vector3(0, 0.2f, 0);
    public string bubbleText = "Hello!";
    public GameObject bubbleInstance;
    
    [Header("Speech Bubble Events")]
    public UnityEvent onSpeechBubbleClicked;
    
    [Header("Menu Reference (Optional)")]
    [Tooltip("If set, pressing the trigger key will directly show this menu. Otherwise, uses the onSpeechBubbleClicked event.")]
    public PatientMenu patientMenu;
    
    void Awake()
    {
        // Auto-find PatientMenu if not set
        if (patientMenu == null)
        {
            patientMenu = FindFirstObjectByType<PatientMenu>();
            if (patientMenu != null)
            {
                Debug.Log("[SpeechBubbleBuilder] Automatically found PatientMenu");
            }
            else
            {
                Debug.LogWarning("[SpeechBubbleBuilder] Could not find PatientMenu in scene. Please assign it manually or ensure PatientMenu exists.");
            }
        }
        
        // Create empty holder
        GameObject bubble = new GameObject("SpeechBubble");
        bubble.transform.SetParent(headTransform);
        bubble.transform.localPosition = offset;
        bubble.transform.localRotation = Quaternion.identity;

        // Billboard
        bubble.AddComponent<Billboard>();

        // Canvas setup
        Canvas canvas = CreateCanvas(bubble.transform);
        AddPanelWithText(canvas.transform, bubbleText);

        // Create a CanvasGroup to handle the visibility
        CanvasGroup canvasGroup = bubble.AddComponent<CanvasGroup>();
        //canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        bubbleInstance = bubble;

        // Hook to hover script
        var hover = GetComponent<PatientHoverInteractable>();
        if (hover != null) hover.speechBubble = bubble;
        
        Debug.Log($"[SpeechBubbleBuilder] Speech bubble initialized on {gameObject.name}");
    }

    void Start()
    {
        Debug.Log($"[SpeechBubbleBuilder] Start called on {gameObject.name}. PatientMenu found: {patientMenu != null}");
    }

    Canvas CreateCanvas(Transform parent)
    {
        GameObject canvasGO = new GameObject("BubbleCanvas");
        canvasGO.transform.SetParent(parent);
        canvasGO.transform.localPosition = Vector3.zero;
        canvasGO.transform.localRotation = Quaternion.identity;
        canvasGO.transform.localScale = Vector3.one * 0.003f;

        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasGO.AddComponent<CanvasScaler>().dynamicPixelsPerUnit = 10f;

        var trackedDeviceRaycaster = canvasGO.AddComponent<TrackedDeviceGraphicRaycaster>();
        trackedDeviceRaycaster.enabled = true;

        return canvas;
    }

    void AddPanelWithText(Transform canvas, string text)
    {
        // Panel
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(canvas);
        panel.transform.localRotation = Quaternion.identity;
        Image image = panel.AddComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 1f);

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(0.5f, 0.2f);
        panelRect.localPosition = Vector3.zero;

        GameObject tail = new GameObject("Tail");
        tail.transform.SetParent(panel.transform);
        tail.transform.localRotation = Quaternion.identity;
        tail.transform.localScale = Vector3.one;

        Image tailImage = tail.AddComponent<Image>();
        tailImage.color = panel.GetComponent<Image>().color; // match color

        RectTransform tailRect = tail.GetComponent<RectTransform>();
        tailRect.sizeDelta = new Vector2(0.1f, 0.1f);
        tailRect.localPosition = new Vector3(0, panelRect.sizeDelta.y / 4f, 0);
        tailRect.localRotation = Quaternion.Euler(0, 0, 45);
        tailRect.anchorMin = new Vector2(0.5f, 0f);
        tailRect.anchorMax = new Vector2(0.5f, 0f);
        tailRect.pivot = new Vector2(0.5f, 1f);

        // Text
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(panel.transform);
        textGO.transform.localRotation = Quaternion.identity;
        TMP_Text tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 0.1f;
        tmp.color = Color.black;
        tmp.alignment = TextAlignmentOptions.Center;

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.sizeDelta = panelRect.sizeDelta;
        textRect.localPosition = Vector3.zero;

        // Add a Button component to the panel to make it clickable
        Button btn = panel.AddComponent<Button>();
        btn.onClick.AddListener(() => onSpeechBubbleClicked?.Invoke());

        // Ensure raycasting is enabled for the panel and its children
        var imageComp = panel.GetComponent<Image>();
        if (imageComp != null)
        {
            imageComp.raycastTarget = true;
        }

        var textComp = textGO.GetComponent<TMP_Text>();
        if (textComp != null)
        {
            textComp.raycastTarget = true;
        }

        var hoverHandler = panel.AddComponent<SpeechBubbleHoverHandler>();
        hoverHandler.hoverTarget = GetComponent<PatientHoverInteractable>();
    }

    public void SetText(string text)
    {
        bubbleInstance.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}