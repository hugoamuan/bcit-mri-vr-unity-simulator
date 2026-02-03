using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public GameObject userSpeechPanel;
    public GameObject patientSpeechPanel;
    public ShowSpeechButtonZone showSpeechButtonZone;
    public string startingText = "Which patient am I scanning next?";
    public string selectionUserText = "Are you last name Doe?";
    public string selectionPatientText = "Yes, that's me";
    public string speechMenuInstruction = "You can press X on the controller or M on the keyboard to open dialogue options.";

    private Image userPanelImg;
    private Image patientPanelImg;
    private TMPro.TextMeshProUGUI userText;
    private TMPro.TextMeshProUGUI patientText;
    private string userSpeechTemplate = "{0}";
    private string defaultText = "...";
    private float patientDialogueDuration = 2f;
    private float fadeDuration = 0.6f;
    private bool userTextActive = false;
    private bool patientTextActive = false;
    private float userDialogueTime = 0;
    private float patientDialogueTime = 0;
    private float speechOpacity = 0.9f;
    private bool speechInstructionGiven = false;

    // Start is called before the first frame update
    void Start()
    {
        userPanelImg = userSpeechPanel.GetComponent<Image>();
        if (userPanelImg == null)
        {
            Debug.LogError("userPanelImg object missing");
        }

        patientPanelImg = patientSpeechPanel.GetComponent<Image>();
        if (patientPanelImg == null)
        {
            Debug.LogError("patientPanelImg object missing");
        }

        userText = userSpeechPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (userText == null)
        {
            Debug.LogError("userText object missing");
        }
        SetUserText(startingText);
        SetUserSpeechVisibility(true);

        patientText = patientSpeechPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (patientText == null)
        {
            Debug.LogError("patientText object missing");
        }
        SetPatientSpeechVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (userTextActive)
        {
            if (userDialogueTime > 0)
            {
                userDialogueTime -= Time.deltaTime;

                if (userDialogueTime < patientDialogueDuration)
                {
                    patientDialogueTime = patientDialogueDuration;
                    patientTextActive = true;
                    SetPatientSpeechVisibility(true);
                }

                if (userDialogueTime < fadeDuration)
                    SetPanelOpacity(userPanelImg, userText, userDialogueTime / fadeDuration);

            }
            else userTextActive = false;
        }

        if (patientTextActive)
        {
            if (patientDialogueTime > 0)
            {
                patientDialogueTime -= Time.deltaTime;

                if (patientDialogueTime < fadeDuration)
                    SetPanelOpacity(patientPanelImg, patientText, patientDialogueTime / fadeDuration);

            }
            else
            {
                patientTextActive = false;
                if (!speechInstructionGiven)
                {
                    speechInstructionGiven = true;
                    SetUserText(speechMenuInstruction);
                    SetUserSpeechVisibility(true);
                }
                
            } 
                
        }
    }

    private void SetPanelOpacity(Image panelImg, TextMeshProUGUI text, float alpha)
    {
        panelImg.color = new Color(
            panelImg.color.r,
            panelImg.color.g,
            panelImg.color.b,
            alpha
        );

        text.faceColor = new Color(
            text.faceColor.r,
            text.faceColor.g,
            text.faceColor.b,
            alpha / speechOpacity
        );
    }

    public void SetUserText()
    {
        userText.text = string.Format(userSpeechTemplate, defaultText);
    }

    public void SetUserText(string text)
    {
        userText.text = string.Format(userSpeechTemplate, text);
    }

    public void SetUserSpeechVisibility(bool isVisible)
    {
        SetPanelOpacity(userPanelImg, userText, speechOpacity);
        userSpeechPanel.SetActive(isVisible);
    }

    public void SetUserSpeechTemplate(string template)
    {
        userSpeechTemplate = template;
    }

    public void SetPatientText(string text)
    {
        patientText.text = text;
    }

    public void SetPatientSpeechVisibility(bool isVisible)
    {
        SetPanelOpacity(patientPanelImg, patientText, speechOpacity);
        patientSpeechPanel.SetActive(isVisible && patientText.text != "");
    }

    public void InitiateDialogue(string dialogueTemplate, string userDialogue, string patientDialogue, float duration)
    {
        if (dialogueTemplate != null && dialogueTemplate.Trim() != "")
        {
            SetUserSpeechTemplate(dialogueTemplate);
        }

        if (userDialogue != null && userDialogue.Trim() != "") 
        {
            SetUserText(userDialogue);
            SetUserSpeechVisibility(true);
            userTextActive = true;
            userDialogueTime = duration;
            if (patientDialogue == null || patientDialogue.Trim() == "")
            {
                SetPatientText("");
            }
        }
        else if (dialogueTemplate != null && dialogueTemplate.Trim() != "")
        {
            SetUserText(defaultText);
            SetUserSpeechVisibility(true);
            userTextActive = true;
            userDialogueTime = duration;
        }

        if (patientDialogue != null && patientDialogue.Trim() != "")
        {
            SetPatientText(patientDialogue);
            if (userDialogue != null && userDialogue.Trim() != "")
            {
                SetPatientSpeechVisibility(false);
            }
            else
            {
                SetPatientText(patientDialogue);
                SetPatientSpeechVisibility(true);
                patientTextActive = true;
                patientDialogueTime = duration;
            }
        }
    }

    public void InitiateSelectionDialogue()
    {
        InitiateDialogue("", selectionUserText, selectionPatientText, 2.5f);
    }
}
