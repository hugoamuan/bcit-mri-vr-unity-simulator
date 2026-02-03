using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ReturnedCheck : BaseCheck
{
    public override bool Check(UnityEngine.Events.UnityAction ContinueClick, UnityEngine.Events.UnityAction GoBackClick = null)
    {
        errorGroups.Clear();
        foreach (var entry in errorGroupEntries)
        {
            if (!errorGroups.ContainsKey(entry.key))
                errorGroups.Add(entry.key, entry.objects);
        }

        string type = dataBanker.GetExamType();
        GameObject[] Errors = errorGroups.ContainsKey(type) ? errorGroups[type] : null;

        if (Errors == null || Errors.Length == 0)
        {
            Debug.LogWarning("No objects in Errors array!");
            return false;
        }

        ContinueClickAction = ContinueClick;
        GoBackClickAction = GoBackClick;

        ClearText();
        AddText("Procedure Feedback", Color.black, true);
        GameObject errorText = AddText("Please fix your errors before continuing", Color.black, false);
        errorText.SetActive(false);

        bool all_Correct = true;

        foreach (GameObject obj in Errors)
        {
            ReturnedInterface checker = obj.GetComponent<ReturnedInterface>();
            if (checker == null)
            {
                Debug.LogError($"{obj.name} missing ReturnedInterface!");
                continue;
            }

            bool isReturned = checker.isReturned();

            try
            {
                AddText(checker.getReturnedLabel() + (isReturned ? " was done" : " was not done"),
                        isReturned ? Color.green : Color.red);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return false;
            }

            all_Correct &= isReturned;
        }

        ErrorPanel.gameObject.SetActive(true);
        ShowButtons(all_Correct, errorText);

        return all_Correct;
    }

    public void ClickRestart()
    {
        if (dataBanker != null)
        {
            dataBanker.SetExamType(null);
            dataBanker.SetGender("Male");
            dataBanker.setFirstCheck(false);
        }

        SceneManager.LoadScene("Room4");
    }
}
