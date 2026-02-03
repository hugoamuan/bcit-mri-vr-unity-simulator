using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used so the feedback report can check if an object is right
/// or wrong in the report.
/// </summary>
public interface CheckerInterface
{
   bool isCorrect();

   string getLabel();
}
