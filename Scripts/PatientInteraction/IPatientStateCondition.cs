using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used to create fail safes in the program and allow the patient
/// to refuse to progress in special cases.
/// </summary>
public interface IPatientStateCondition
{
    public bool IsStateChangeAllowed();

    public string GetRefusalMessage();
}
