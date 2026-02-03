using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class ErrorCheck : BaseCheck
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
        if (type == null)
        {
            Debug.LogError("Exam type not set.");
            return false;
        }

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
            CheckerInterface checker = obj.GetComponent<CheckerInterface>();
            if (checker == null)
            {
                Debug.LogError($"{obj.name} missing CheckerInterface!");
                continue;
            }

            bool isCorrect = checker.isCorrect();

            try
            {
                AddText(checker.getLabel() + (isCorrect ? " is correct" : " is not correct"),
                        isCorrect ? Color.green : Color.red);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return false;
            }

            all_Correct &= isCorrect;
        }

        ErrorPanel.gameObject.SetActive(true);
        ShowButtons(all_Correct, errorText);

        return all_Correct;
    }
}
