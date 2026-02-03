using UnityEngine;

// Controls a touchscreen menu system, allowing navigation between different panels
public class TouchscreenMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject comfortMenuPanel;  // Panel for the Comfort menu
    [SerializeField] private GameObject physioMenuPanel;   // Panel for the Physio menu
    [SerializeField] private GameObject settingsMenuPanel; // Panel for the Settings menu
    [SerializeField] private GameObject patientPanel; // // Panel related to patient panel information
    [SerializeField] private GameObject sidePanel; // Side panel beside patient panel

    // Toggle the Patient Panel
    void Start()
    {
        TogglePatientPanel();
    }

    // Show the Comfort menu and hide others
    public void ShowComfortMenu()
    {
        comfortMenuPanel.SetActive(true); // Show comfort menu
        physioMenuPanel.SetActive(false); // Hide physio menu
        settingsMenuPanel.SetActive(false); // Hide settings menu
        TogglePatientPanel(); // Toggle patient panel visibility
    }

    // Show the Physio menu and hide others
    public void ShowPhysioMenu()
    {
        comfortMenuPanel.SetActive(false); // Hide comfort menu
        physioMenuPanel.SetActive(true); // Show physio menu
        settingsMenuPanel.SetActive(false); // Show settings menu
        TogglePatientPanel(); // Toggle patient panel visibility
    }

    // Show the Settings menu and hide others
    public void ShowSettingsMenu()
    {
        comfortMenuPanel.SetActive(false); // Hide comfort menu
        physioMenuPanel.SetActive(false); // Show physio menu
        settingsMenuPanel.SetActive(true); // Show settings menu
        TogglePatientPanel(); // Toggle patient panel visibility
    }

    // Toggles the visibility of the patient panel and side panel
    public void TogglePatientPanel()
    {
        if (patientPanel != null)
        {
            patientPanel.SetActive(!patientPanel.activeSelf); // Toggle patient panel visibility
            sidePanel.SetActive(!sidePanel.activeSelf); // Toggle side panel visibility
        }
    }
}