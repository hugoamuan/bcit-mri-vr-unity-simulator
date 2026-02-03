using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages the light indicators for a cluster of buttons, including up, down, home, and crosshair buttons.
/// This class inherits from <see cref="ButtonLightBase"/> and initializes button lights using a dictionary.
/// It ensures that each button light's state is properly tracked and managed.
/// </summary>
public class ButtonClusterLightController : ButtonLightBase
{
    // Variables that indiate light for the up, down, home, and crosshair buttons
    public GameObject upButtonLight;
    public GameObject downButtonLight;
    public GameObject homeButtonLight;
    public GameObject crosshairButtonLight;
    // Button names corresponding to the assigned lights
    public string[] buttonNames = { "Up", "Down", "Home", "Crosshair" };
    /// <summary>
    /// Initializes the button light system upon awakening.
    /// - Assigns button lights to an array.
    /// - Calls InitializeLights to set up the lights in the dictionary.
    /// </summary>
    private void Awake()
    {
        buttonLightObjects = new GameObject[] { upButtonLight, downButtonLight, homeButtonLight, crosshairButtonLight };
        InitializeLights(buttonNames);
    }
}