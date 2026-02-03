using UnityEngine;

/// <summary>
/// Controls the display and positioning of a floating image in front of the main camera.
/// </summary>
public class FloatingImageController : MonoBehaviour
{
    public Camera mainCamera; // Assign the Main Camera in the Inspector
    public GameObject imageObject; // Assign the Canvas GameObject in the Inspector

    void Start()
    {
        imageObject.SetActive(false); // Initially hide the image
    }

    public void ShowImage()
    {
        imageObject.SetActive(true);
        PositionImage();
    }

    public void HideImage()
    {
        imageObject.SetActive(false);
    }

    void Update()
    {
        if (imageObject.activeSelf)
        {
            PositionImage();
        }
    }

    private void PositionImage()
    {
        // Position the image in front of the main camera
        imageObject.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 2f;
        imageObject.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward, Vector3.up);
    }
}