using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// This class represents a basic light button that interacts with a <see cref="ButtonLightController"/> to control the state of a light.
/// It provides methods to turn the light on and off, and it tracks the current state of the light (on/off).
/// The light's state is stored and can be retrieved using the <see cref="getState"/> method.
/// </summary>
public class BasicLightButton : MonoBehaviour
{
    // The controller that controllers that manages light button interactions
    public ButtonLightBase buttonLightController;
    // The light object being controlled
    public GameObject lightObject;
    public GameObject externalLightObject = null;
    // The name of the button
    public string buttonName;
    // Tracks the light status
    protected bool isOn = false;
    // Initializes the button state based on the active status of the light object
    public void Start()
    {
        // Set initial state to match the light object's active state
        isOn = lightObject.activeSelf;
    }

    public void Update()
    {
        if (externalLightObject == null)
        {
            return;
        }
        bool externalState = externalLightObject.GetComponent<Light>().enabled;
        if (externalState != isOn)
        {
            if (isOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }

    // Returns the state of the light
    public bool getState()
    {
        return isOn;
    }

    // Turns the light on by calling the ButtonLightController
    public void TurnOn()
    {
        // Turns on the light
        buttonLightController.TurnButtonOn(buttonName);
        // Sets light status
        isOn = true;
    }

    // Turns the light off by calling the ButtonLightController
    public void TurnOff()
    {
        // Turns off the light
        buttonLightController.TurnButtonOff(buttonName);
        // Sets light status
        isOn = false;
    }
}
