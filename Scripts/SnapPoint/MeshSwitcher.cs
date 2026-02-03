using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// Manages the interaction and visibility of a object in an XR environment.
/// Enables an object when grabbed and disables another object when grabbed.
/// Disables itself and enables the target object's MeshRenderer when entering a trigger collider.
/// </summary>
public class MeshSwitcher : MonoBehaviour
{
    public GameObject targetObject; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            gameObject.SetActive(false); 
            targetObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
