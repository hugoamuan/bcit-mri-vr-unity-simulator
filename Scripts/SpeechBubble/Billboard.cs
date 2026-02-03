using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            Vector3 dir = transform.position - Camera.main.transform.position;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
