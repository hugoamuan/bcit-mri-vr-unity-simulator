using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartScanManager : MonoBehaviour
{
    [SerializeField] private AudioSource scannerAudioSource;
    public ErrorCheck ErrorChecker;
    public GameObject Coils;
    public GameObject TissueObject;

    [Header("UI Panels")]
    [SerializeField] private Transform panel2;
    [SerializeField] private Transform finalPanel2;
    [SerializeField] private GameObject ErrorTextPrefab;

    private void Start()
    {
        if (scannerAudioSource != null)
        {
            scannerAudioSource.Stop();
        }
    }

    public void StartScan()
    {
        ErrorChecker.Check(OnContinueClick, () => { });
    }

    public void OnContinueClick()
    {
        scannerAudioSource.Play();
        MovePanel(panel2, finalPanel2, "Patient Prep Results");
    }
    private void MovePanel(Transform sourcePanel, Transform targetParent, string newTitle)
    {
        if (!sourcePanel || !targetParent)
        {
            Debug.LogError("Missing panel references during move.");
            return;
        }

        // Clear any previous children in the target parent
        foreach (Transform child in targetParent)
            Destroy(child.gameObject);

        // Re-parent the entire source panel to the new parent
        sourcePanel.SetParent(targetParent, false);
        sourcePanel.localPosition = Vector3.zero;
        sourcePanel.localRotation = Quaternion.identity;
        sourcePanel.localScale = Vector3.one;

        // Make sure there's at least one child (content panel)
        if (sourcePanel.childCount == 0)
        {
            Debug.LogWarning("Source panel has no content.");
            return;
        }

        Transform contentPanel = sourcePanel.GetChild(0);

        // Replace first entry (title) and second entry (description)
        if (contentPanel.childCount > 0)
        {
            Destroy(contentPanel.GetChild(0).gameObject);
            Destroy(contentPanel.GetChild(1).gameObject);
            GameObject newTitleText = AddText(newTitle, contentPanel, Color.black, true);
            newTitleText.transform.SetSiblingIndex(0);
        }

        // Deactivate the last child of the source panel (assumed to be the button row)
        if (sourcePanel.childCount > 1)
        {
            Transform buttonRow = sourcePanel.GetChild(sourcePanel.childCount - 1);
            buttonRow.gameObject.SetActive(false);
        }
    }

    private GameObject AddText(string text, Transform parent, Color color, bool isTitle = false)
    {
        if (!ErrorTextPrefab) throw new MissingReferenceException("Missing ErrorTextPrefab");

        GameObject obj = Instantiate(ErrorTextPrefab, parent);
        TMP_Text tmp = obj.GetComponent<TMP_Text>();

        if (!tmp) throw new MissingComponentException("ErrorTextPrefab missing TMP_Text");

        tmp.text = isTitle ? $"<style=\"Title\">{text}</style>" : text;
        tmp.color = color;

        return obj;
    }
}
