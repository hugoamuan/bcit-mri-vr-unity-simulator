using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Displays a visual indicator (ghost preview) when a matching coil object enters the snap point's trigger area.
/// The indicator is only shown if the snap conditions are met, providing feedback about whether snapping is allowed.
/// </summary>
public class SnapGhost : MonoBehaviour
{
    private SnapPoint snapPoint;
    
    private void Awake()
    {
        snapPoint = GetComponentInParent<SnapPoint>();
    }
    
    /// <summary>
    /// Shows indicator if coil matches mesh and conditions are met.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coil"))
        {
            if (gameObject.GetComponent<MeshFilter>().sharedMesh == other.gameObject.GetComponentInParent<MeshFilter>().sharedMesh)
            {
                if (CheckSnapConditions())
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    
    /// <summary>
    /// Checks snap conditions from SnapPoint to determine if the indicator should be shown.
    /// </summary>
    /// <returns>True if conditions are met or no conditions exist, false otherwise.</returns>
    private bool CheckSnapConditions()
    {
        if (snapPoint == null)
        {
            return false;
        }        
        return snapPoint.CheckSnapConditions();
    }
}



