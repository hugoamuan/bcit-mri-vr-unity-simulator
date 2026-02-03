using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PatientMenuButton : MonoBehaviour
{
    public int buttonIndex;
    private Vector3 endPosition;
    private float buttonOffset = -20f;
    private bool isAnimatingIn = false;
    private bool isAnimatingOut = false;
    private float animationDuration = 0.15f;
    private float animationTime;

    void Update()
    {
        if (isAnimatingIn)
        {
            animationTime += Time.deltaTime;
            float t = Mathf.Clamp01(animationTime / animationDuration);
            transform.localPosition = Vector3.Lerp(Vector3.zero, endPosition, t);

            if (t >= 1f)
            {
                isAnimatingIn = false;
            }
        }

        if (isAnimatingOut)
        {
            animationTime += Time.deltaTime;
            float t = Mathf.Clamp01(animationTime / animationDuration);
            transform.localPosition = Vector3.Lerp(endPosition, Vector3.zero, t);
            if (t >= 1f)
            {
                isAnimatingOut = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void Initialize(PatientMenuItem menuItem, DialogueController dialogueController)
    {
        GetComponent<UnityEngine.UI.Button>()
            .onClick.AddListener(() => {
                
                if (menuItem.label != "cancel")
                {
                    dialogueController.InitiateDialogue(
                        menuItem.dialogueTemplate,
                        menuItem.userDialogueText, 
                        menuItem.patientDialogueText, 
                        menuItem.dialogueDuration
                    );
                }
                menuItem.onclickCallback(menuItem.targetStateLabel); 
            });
        //if (menuItem.icon != null)
        //{
        //    GetComponent<UnityEngine.UI.Image>().sprite = menuItem.icon;
        //}
        GetComponentInChildren<TextMeshProUGUI>().text = menuItem.hintText;
        endPosition = new Vector3(0, buttonOffset * buttonIndex, 0);
        gameObject.SetActive(false);
    }

    public void AnimateIn()
    {
        transform.localPosition = Vector3.zero;
        isAnimatingIn = true;
        animationTime = 0;
        gameObject.SetActive(true);
    }

    public void AnimateOut() 
    {
        transform.localPosition = endPosition;
        isAnimatingOut = true;
        animationTime = 0;
    }
}
