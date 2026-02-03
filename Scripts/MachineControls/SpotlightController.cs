using UnityEngine;

/// <summary>
/// Controls a spotlight in a Unity scene.
/// Provides functionality to toggle the spotlight on and off.
/// </summary>
public class SpotlightController : MonoBehaviour, CheckerInterface
{
    private bool turnedOnOnce = false;
    public Light spotlight;
    public void ToggleSpotlight()
    {
        if (!turnedOnOnce)
        {
            turnedOnOnce = true;
        }
        if (spotlight != null)
        {
            spotlight.enabled = !spotlight.enabled;
        }
    }
    public bool isCorrect()
    {
        return turnedOnOnce;
    }
    public string getLabel()
    {
        return "Landmarking";
    }
}
