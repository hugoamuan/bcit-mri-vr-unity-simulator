using UnityEngine;

public class HygieneButton : MonoBehaviour
{
    [SerializeField] private SanitizeHands manager;
    
    private void Start()
    {
        // Auto-find manager if not assigned
        if (manager == null)
            manager = FindObjectOfType<SanitizeHands>();
    }

    public void OnMouseDown()
    {
        if (manager != null)
            manager.HandleButtonClick(transform);
    }
}