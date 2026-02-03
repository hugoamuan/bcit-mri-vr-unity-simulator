using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Base class for managing button lights. Provides common functionality to initialize, turn on, and turn off button lights.
/// Stores button states and their corresponding GameObjects in dictionaries.
/// Derived classes should initialize the buttonLights dictionary before calling InitializeLights().
/// </summary>
public abstract class ButtonLightBase : MonoBehaviour
{
    // Array of button light GameObjects that will be assigned in the inspector.
    protected GameObject[] buttonLightObjects;
    // Dictionary to store button names and their corresponding light objects.
    protected Dictionary<string, GameObject> buttonLights;
    // Dictionary to track the on/off state of each button light.
    protected Dictionary<string, bool> buttonStates;
    /// <summary>
    /// Initializes the button lights by mapping button names to their respective GameObjects.
    /// Ensures that each button has a corresponding GameObject and logs debug messages for tracking.
    /// </summary>
    /// <param name="buttonNames">An array of button names corresponding to the lights.</param>
    protected virtual void InitializeLights(string[] buttonNames)
    {
        // Check if button names match the button light objects in length
        if (buttonNames.Length != buttonLightObjects.Length)
        {
            Debug.LogError("Button names and button light objects arrays do not match in length!");
            return;
        }
        
        // Initialize dictionaries
        buttonLights = new Dictionary<string, GameObject>();
        for (int i = 0; i < buttonNames.Length; i++)
        {
            buttonLights.Add(buttonNames[i], buttonLightObjects[i]);
        }
        buttonStates = new Dictionary<string, bool>();
        foreach (var button in buttonLights)
        {
            buttonStates[button.Key] = button.Value.activeSelf;
        }
    }
    /// <summary>
    /// Turns on the specified button light if it exists.
    /// </summary>
    /// <param name="buttonName">The name of the button whose light should be turned on.</param>
    public virtual void TurnButtonOn(string buttonName) {
        if (buttonLights.ContainsKey(buttonName))
        {
            // Activvate the light
            buttonLights[buttonName].SetActive(true);
            // Update the state
            buttonStates[buttonName] = true;
            // Debug.Log($"Button light turned ON: {buttonName}");
        }
        else
        {
            Debug.LogWarning("Button light not found: " + buttonName);
        }
    }
    /// <summary>
    /// Turns off the specified button light if it exists.
    /// </summary>
    /// <param name="buttonName">The name of the button whose light should be turned off.</param>
    public virtual void TurnButtonOff(string buttonName) {
        if (buttonLights.ContainsKey(buttonName))
        {
            // Deactive the light
            buttonLights[buttonName].SetActive(false);
            // Update the state
            buttonStates[buttonName] = false;
            // Debug.Log($"Button light turned OFF: {buttonName}");
        }
        else
        {
            Debug.LogWarning("Button light not found: " + buttonName);
        }
    }
}

