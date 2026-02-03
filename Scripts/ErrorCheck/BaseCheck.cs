using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public abstract class BaseCheck : MonoBehaviour
{
    public List<ErrorGroupEntry> errorGroupEntries = new List<ErrorGroupEntry>();
    protected Dictionary<string, GameObject[]> errorGroups = new Dictionary<string, GameObject[]>();

    public GameObject ErrorTextPrefab;
    public Transform ErrorTextContainer;
    public Transform ErrorPanel;
    public GameObject ButtonRow;
    public GameObject ContinueButton;
    protected UnityEngine.Events.UnityAction ContinueClickAction;
    public GameObject GoBackButton;
    protected UnityEngine.Events.UnityAction GoBackClickAction;
    public DataBanker dataBanker;

    public abstract bool Check(UnityEngine.Events.UnityAction ContinueClick, UnityEngine.Events.UnityAction GoBackClick = null);

    public void ClickContinue()
    {
        ContinueClickAction?.Invoke();
        // DisablePanel();
    }

    public void ClickGoBack()
    {
        GoBackClickAction?.Invoke();
        DisablePanel();
    }

    protected void ShowButtons(bool allCorrect, GameObject errorText)
    {
        ButtonRow.SetActive(true);

        if (!allCorrect)
        {
            ContinueButton.SetActive(true);
            GoBackButton.SetActive(GoBackClickAction != null);
            errorText.SetActive(true);
        }
        else
        {
            GoBackButton.SetActive(false);
            errorText.SetActive(false);
            ContinueButton.SetActive(true);

            if (ColorUtility.TryParseHtmlString("#69EA70", out Color newColor))
                ContinueButton.GetComponent<Image>().color = newColor;
            else
                Debug.LogWarning("Invalid color format!");
        }
    }

    protected GameObject AddText(string text, Color color, bool isTitle = false)
    {
        GameObject errorTextObj = Instantiate(ErrorTextPrefab, ErrorTextContainer);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ErrorTextContainer.GetComponent<RectTransform>());

        TMP_Text errorText = errorTextObj.GetComponent<TMP_Text>();
        if (errorText == null)
        {
            throw new Exception("ErrorTextPrefab missing TMP_Text component");
        }

        errorText.text = isTitle ? $"<style=\"Title\">{text}</style>" : text;
        errorText.color = color;

        return errorTextObj;
    }

    protected void ClearText()
    {
        foreach (Transform child in ErrorTextContainer)
        {
            Destroy(child.gameObject);
        }
    }

    protected void DisablePanel()
    {
        ErrorPanel.gameObject.SetActive(false);
        ButtonRow.SetActive(false);
        ContinueButton.SetActive(false);
        GoBackButton.SetActive(false);
    }
}
