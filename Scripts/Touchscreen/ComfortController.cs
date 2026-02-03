using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manages lighting brightness and fan speed through UI sliders
public class ComfortController : MonoBehaviour
{
    [Header("Light Controls")]
    [SerializeField] private Light[] lights; // Array of lights to control
    [SerializeField] private Slider brightnessSlider; // Slider for brightness control
    [SerializeField] private TextMeshProUGUI brightnessText; // UI text displaying brightness level

    [Header("Fan Controls")]
    [SerializeField] private Slider fanSpeedSlider; // Slider for fan speed control
    [SerializeField] private TextMeshProUGUI fanSpeedText; // UI text displaying fan speed
    [SerializeField] private AudioSource fanAudioSource; // Fan sound source
    [SerializeField] private float maxFanVolume = 0.5f; // Maximum volume of fan sound

    
    private void Start()
    {
        // Initialize brightness slider
        if (brightnessSlider != null)
        {
            brightnessSlider.value = 0; // Default 0 brightness
            brightnessSlider.wholeNumbers = true; // Restrict values to whole numbers
            brightnessSlider.onValueChanged.AddListener(UpdateLightsBrightness); // link function to slider changes
            UpdateBrightnessText(brightnessSlider.value); // Update displayed brithness
        }
        // Initialize fan speed slider
        if (fanSpeedSlider != null)
        {
            fanSpeedSlider.value = 0; // Default 0 speed
            fanSpeedSlider.wholeNumbers = true; // Restrict values to whole numbers
            fanSpeedSlider.onValueChanged.AddListener(UpdateFanSpeed); // Link function to slider changes
            UpdateFanSpeedText(fanSpeedSlider.value); // Update displayed fan speed
        }

        // Ensure fan sound is stopped initially
        if (fanAudioSource != null)
        {
            // Enable looping sound
            fanAudioSource.loop = true;
            // Make sure it's not playing at start
            fanAudioSource.Stop();
        }
    }

    // Adjusts the brightness of all lights based on slider value
    private void UpdateLightsBrightness(float value)
    {
        // Scan each light
        foreach (Light light in lights)
        {
            if (light != null)
            {
                // Scale brightness (slider 0-100 → intensity 0.0-1.0)
                light.intensity = (float)(value * 0.01);
            }
        }
        // Update brightness UI text
        UpdateBrightnessText(value);
    }

    // Update brightness text
    private void UpdateBrightnessText(float value)
    {
        if (brightnessText != null)
        {
            // Show "Off" if 0, otherwise percentage
            brightnessText.text = value == 0 ? "Off" : $"{Mathf.RoundToInt(value)}%";
        }
    }

    // Update fan speed and sound
    private void UpdateFanSpeed(float value)
    {
        // Prevent errors if slider is missing
        if (fanSpeedSlider != null)
        {
            if (value > 0) // If fan speed is above 0
            {
                if (!fanAudioSource.isPlaying)
                {
                    // Start fan sound if not already playing
                    fanAudioSource.Play();
                }
                // Adjust pitch and volume based on fan speed
                fanAudioSource.pitch = Mathf.Lerp(0.8f, 1.5f, value / fanSpeedSlider.maxValue);
                fanAudioSource.volume = Mathf.Lerp(0.1f, maxFanVolume, value / fanSpeedSlider.maxValue);
            }
            else
            {
                // Stop fan sound if speed is 0
                fanAudioSource.Stop();
            }
            // Update fan speed UI text
            UpdateFanSpeedText(value);
        }
    }

    // Update fan speed text
    private void UpdateFanSpeedText(float value)
    {
        if (fanSpeedText != null)
        {
            // Show "Off" if 0, otherwise numeric speed
            fanSpeedText.text = value == 0 ? "Off" : $"{Mathf.RoundToInt(value)}";
        }
    }
}
