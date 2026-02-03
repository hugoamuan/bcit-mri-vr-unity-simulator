using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanitizeHands : MonoBehaviour, CheckerInterface
{
    private DataBanker dataBanker;
    private bool correct = false;
    
    // Effect settings
    [Header("Effect Settings")]
    [SerializeField] private float spawnOffsetY = 0.0f;
    [SerializeField] private float effectDuration = 0.5f; // Increased for slower fall
    [SerializeField] private float bubbleSize = 0.03125f;
    [SerializeField] private int bubbleCount = 10;
    [SerializeField] private Color effectColor = new Color(0.8f, 0.9f, 1f, 0.7f);

    // Physics settings for slower fall
    [Header("Physics Settings")]
    [SerializeField] private float minFallSpeed = 0.1f; // Reduced from 0.3f
    [SerializeField] private float maxFallSpeed = 0.3f; // Reduced from 0.8f
    [SerializeField] private float bubbleMass = 0.02f; // Lighter bubbles
    [SerializeField] private float bubbleDrag = 3f; // Increased drag for slower movement
    [SerializeField] private float torqueAmount = 2f; // Reduced rotation

    // Parent containers
    [Header("Button References")]
    [SerializeField] private Transform sanitizerContainer;

    // Client asked to remove the soap container from the required sanitzation steps.
    // [SerializeField] private Transform soapContainer;

    void Start()
    {
        dataBanker = FindObjectOfType<DataBanker>();
        if (sanitizerContainer == null)
            sanitizerContainer = GameObject.Find("SanitizerInter")?.transform;
        // if (soapContainer == null)
        //     soapContainer = GameObject.Find("SoapInter")?.transform;
    }

    public void HandleButtonClick(Transform buttonTransform)
    {
        correct = true;
        CreateBubbleEffect(buttonTransform);
    }

    private void CreateBubbleEffect(Transform buttonTransform)
    {
        GameObject bubbleContainer = new GameObject("BubbleEffect");
        bubbleContainer.transform.position = buttonTransform.position + Vector3.up * spawnOffsetY;
        
        for (int i = 0; i < bubbleCount; i++)
        {
            StartCoroutine(CreateBubble(bubbleContainer.transform));
        }
        
        Destroy(bubbleContainer, effectDuration);
    }

    private IEnumerator CreateBubble(Transform parent)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.1f, 0.1f), // Tighter spread
            Random.Range(-0.02f, 0.02f),
            Random.Range(-0.1f, 0.1f)
        );
        
        GameObject bubble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bubble.transform.SetParent(parent);
        bubble.transform.localPosition = randomOffset;
        bubble.transform.localScale = Vector3.one * bubbleSize;
        
        Renderer renderer = bubble.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = effectColor;
        renderer.material.SetFloat("_Metallic", 0.3f);
        renderer.material.SetFloat("_Glossiness", 0.8f);
        
        Rigidbody rb = bubble.AddComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity for more control
        rb.mass = bubbleMass;
        rb.drag = bubbleDrag;
        
        // Apply gentle downward force
        rb.AddForce(Vector3.down * Random.Range(minFallSpeed, maxFallSpeed), ForceMode.Impulse);
        
        // Gentle rotation
        rb.AddTorque(new Vector3(
            Random.Range(-torqueAmount, torqueAmount),
            Random.Range(-torqueAmount, torqueAmount),
            Random.Range(-torqueAmount, torqueAmount)
        ));
        
        Destroy(bubble, effectDuration);
        yield return null;
    }

    public bool isCorrect() => correct;
    public string getLabel() => "Hand Hygiene";
}