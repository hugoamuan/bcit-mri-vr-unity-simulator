using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls the button light indicators for various buttons, including up, down, park, dock, power, and hand buttons.
/// This class inherits from <see cref="ButtonLightBase"/> and initializes button lights using a dictionary.
/// It manages the activation and deactivation of lights based on button states.
/// Additionally, it ensures that non-power buttons only activate when the power button is turned on.
/// </summary>
/// 
public class ButtonLightController : ButtonLightBase
{
    // References to individual button light GameObjects
    public GameObject upButtonLight;
    public GameObject downButtonLight;
    public GameObject parkButtonLight;
    public GameObject dockButtonLight;
    public GameObject powerButtonLight;
    public GameObject rightHandButtonLight;
    public GameObject leftHandButtonLight;
    // Parent object containing non-power button lights (toggles based on power button state)
    public GameObject NonPowerButtons;
    // List of button names corresponding to the lights
    public string[] buttonNames = { "Up", "Down", "Park", "Dock", "Power", "RightHand", "LeftHand" };
    /// <summary>
    /// Called when the script is first loaded. Initializes button light objects and sets up their states.
    /// </summary>
    private void Awake()
    {
        // Assigns the button lights into an array that matches the order of buttonNames.
        buttonLightObjects = new GameObject[] { upButtonLight, downButtonLight, parkButtonLight, dockButtonLight, powerButtonLight, rightHandButtonLight, leftHandButtonLight };
        // Calls the base class method to initialize the button lights using the button names.
        InitializeLights(buttonNames);
    }
    /// <summary>
    /// Turns on a specific button light. If the "Power" button is turned on, all non-power buttons become active.
    /// </summary>
    /// <param name="buttonName">The name of the button light to turn on.</param>
    public override void TurnButtonOn(string buttonName)
    {
        // Check if the button exists in the dictionary
        if (buttonLights.ContainsKey(buttonName))
        {
            // Enable the light associated with the button
            buttonLights[buttonName].SetActive(true);
            // Update the button's state to reflect that it is on
            buttonStates[buttonName] = true;
            // If the power button is turned on, activate all non-power buttons
            if (buttonName == "Power")
            {
                NonPowerButtons.SetActive(true);
            }
        }
        else
        {
            // Display a warning if the button name is not found in the dictionary
            Debug.LogWarning("Button light not found: " + buttonName);
        }
    }
    /// <summary>
    /// Turns off a specific button light. If the "Power" button is turned off, all non-power buttons become inactive.
    /// </summary>
    /// <param name="buttonName">The name of the button light to turn off.</param>
    public override void TurnButtonOff(string buttonName)
    {
        // Check if the button exists in the dictionary
        if (buttonLights.ContainsKey(buttonName))
        {
            // Disable the light associated with the button
            buttonLights[buttonName].SetActive(false);
            // Update the button's state to reflect that it is off
            buttonStates[buttonName] = false;
            // If the power button is turned off, deactivate all non-power buttons
            if (buttonName == "Power")
            {
                NonPowerButtons.SetActive(false);
            }
        }
        else
        {
            // Display a warning if the button name is not found in the dictionary
            Debug.LogWarning("Button light not found: " + buttonName);
        }
    }
}