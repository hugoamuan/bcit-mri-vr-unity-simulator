using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VRBlink : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform topEyelid;
    public RectTransform bottomEyelid;

    [Header("Timing")]
    public float blinkDuration = 0.2f; // Time to close/open
    public float holdDuration = 0.1f;  // Time eyes stay closed

    [Header("Motion & Visuals")]
    public float eyelidMaxSize = 600f; // Total distance eyelids travel toward center
    public float targetAlpha = 1f;     // Max opacity during blink (0–1)

    // Internal state
    Vector2 topStartPos, bottomStartPos;
    Image topImage, bottomImage;

    void Start()
    {
        // Cache image components
        topImage = topEyelid.GetComponent<Image>();
        bottomImage = bottomEyelid.GetComponent<Image>();

        // Set dynamic size (height) for each eyelid
        float eyelidHeight = eyelidMaxSize / 2f;
        Vector2 size = new Vector2(topEyelid.sizeDelta.x, eyelidHeight);
        topEyelid.sizeDelta = size;
        bottomEyelid.sizeDelta = size;

        // Position eyelids off-screen vertically
        Vector2 center = Vector2.zero; // (0,0) = center in anchoredPosition space
        topStartPos = center + new Vector2(0, eyelidMaxSize / 2f);
        bottomStartPos = center - new Vector2(0, eyelidMaxSize / 2f);

        topEyelid.anchoredPosition = topStartPos;
        bottomEyelid.anchoredPosition = bottomStartPos;

        // Set initial transparency
        SetAlpha(0f);
    }

    public void Blink()
    {
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        float halfDistance = eyelidMaxSize / 2f;

        // Slide in & fade in
        yield return StartCoroutine(MoveEyelids(0, halfDistance));
        yield return new WaitForSeconds(holdDuration);
        // Slide out & fade out
        yield return StartCoroutine(MoveEyelids(halfDistance, 0));
    }

    IEnumerator MoveEyelids(float from, float to)
    {
        float timer = 0f;
        while (timer < blinkDuration)
        {
            float t = timer / blinkDuration;
            float current = Mathf.Lerp(from, to, t);
            float alpha = Mathf.Lerp(0f, targetAlpha, to > from ? t : 1f - t);

            // Move eyelids
            topEyelid.anchoredPosition = topStartPos - new Vector2(0, current);
            bottomEyelid.anchoredPosition = bottomStartPos + new Vector2(0, current);

            // Fade alpha
            SetAlpha(alpha);

            timer += Time.deltaTime;
            yield return null;
        }

        // Final positions & alpha
        topEyelid.anchoredPosition = topStartPos - new Vector2(0, to);
        bottomEyelid.anchoredPosition = bottomStartPos + new Vector2(0, to);
        SetAlpha(to == 0 ? 0f : targetAlpha);
    }

    void SetAlpha(float alpha)
    {
        Color topColor = topImage.color;
        Color bottomColor = bottomImage.color;

        topColor.a = alpha;
        bottomColor.a = alpha;

        topImage.color = topColor;
        bottomImage.color = bottomColor;
    }
}
