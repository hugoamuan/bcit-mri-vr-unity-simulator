using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatientMenu : MonoBehaviour
{
    public string iconBaseFilepath;
    public GameObject buttonPrefab;
    public GameObject menuObject;
    public DialogueController dialogueController;
    public Sprite cancelSprite;
    public List<PatientMenuItem> allMenuItems;

    private List<PatientMenuItem> currentMenuItems = new();
    private PatientMenuItem cancelItem;
    private bool isEnabled = false;
    private bool isVisible = false;

    void Start()
    {
        cancelItem = new PatientMenuItem();
        cancelItem.label = "cancel";
        cancelItem.hintText = "Cancel";
        cancelItem.icon = cancelSprite;
        cancelItem.onclickCallback = (string unused) => HideMenu();
    }

    void Update()
    {
        
    }

    public void Disable()
    {
        isEnabled = false;
        ClearMenu();
        //speechBubbleBuilder.bubbleInstance.SetActive(false);
    }

    public void Enable()
    {
        isEnabled = true;
        //speechBubbleBuilder.bubbleInstance.SetActive(true);
    }

    public void SetItems(List<string> items, PatientMenuItem.callback stateCallback, string? examTypeFilter = null)
    {
        ClearMenu();
        if (items != null)
        {
            int buttonCount = 1;
            foreach (string item in items)
            {
                try
                {
                    AddItem(item, stateCallback, buttonCount, examTypeFilter);
                    buttonCount++;
                }
                catch(System.Exception e)
                {
                    Debug.LogWarning(e);
                }
                
            }

            currentMenuItems.Add(cancelItem);
            cancelItem.button = Instantiate(buttonPrefab, menuObject.transform).GetComponent<PatientMenuButton>();
            cancelItem.button.buttonIndex = buttonCount;
            cancelItem.button.Initialize(cancelItem, dialogueController);
        }
    }

    private void AddItem(string itemName, PatientMenuItem.callback stateCallback, int buttonIndex, string? examTypeFilter = null)
    {
        foreach (PatientMenuItem item in allMenuItems) 
        {
            if (item.label.Equals(itemName) && 
                (examTypeFilter == null || item.examFilter == null || 
                item.examFilter.Equals("") || item.examFilter.Equals(examTypeFilter)))
            {
                item.onclickCallback = stateCallback;
                currentMenuItems.Add(item);
                item.button = Instantiate(buttonPrefab, menuObject.transform).GetComponent<PatientMenuButton>();
                item.button.buttonIndex = buttonIndex;
                item.button.Initialize(item, dialogueController);
                return;
            }
        }
        throw new System.Exception($"Could not add menu item: {itemName} is not a valid menu item name");
    }

    public void ShowMenu()
    {
        if (!isEnabled) return;

        if (isVisible)
        {
            HideMenu();
            return;
        }

        foreach (PatientMenuItem item in currentMenuItems)
        {
            item.button.AnimateIn();
        }
        isVisible = true;
    }

    public void HideMenu()
    {
        if (!isEnabled || !isVisible) return;

        foreach (PatientMenuItem item in currentMenuItems)
        {
            item.button.AnimateOut();
        }
        isVisible= false;
    }

    private void ClearMenu()
    {
        isVisible = false;
        currentMenuItems.Clear();
        foreach (Transform child in menuObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}

[System.Serializable]
public class PatientMenuItem
{
    public string label;
    public string targetStateLabel;
    public string hintText;
    public string dialogueTemplate;
    public string userDialogueText;
    public string patientDialogueText;
    public float dialogueDuration;
    public Sprite icon;
    [Tooltip("Menu item will only show for this exam type (leave blank for no filter)")]
    public string examFilter;
    [HideInInspector] public delegate void callback(string label);
    [HideInInspector] public callback onclickCallback;
    [HideInInspector] public PatientMenuButton button;
}
