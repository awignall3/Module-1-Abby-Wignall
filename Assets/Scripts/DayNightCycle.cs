using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Tooltip("The directional light that acts as the sun/moon.")]
    public Light sunLight;

    [Tooltip("How many minutes a full in-game day lasts.")]
    public float secondsPerDay = 60f; 

    [Tooltip("Curve to control the light intensity over the day (0 to 1).")]
    public AnimationCurve lightIntensityCurve;

    private float timeOfDay;
    private float sunInitialIntensity;

    void Start()
    {
        if (sunLight != null)
        {
            sunInitialIntensity = sunLight.intensity;
        }
    }

    void Update()
    {
        timeOfDay += Time.deltaTime / secondsPerDay;
        timeOfDay %= 1f; // Keep the value between 0 and 1

        sunLight.transform.localRotation = Quaternion.Euler(timeOfDay * 360f, 0f, 0f);

        UpdateLighting(timeOfDay);
    }

    void UpdateLighting(float timePercent)
    {
        // Change the sun's intensity using an AnimationCurve in the Inspector
        float intensity = lightIntensityCurve.Evaluate(timePercent) * sunInitialIntensity;
        sunLight.intensity = intensity;

        // Change ambient light color/intensity through RenderSettings
        // You can use a Gradient or an AnimationCurve for Color as well
        if (timePercent > 0.25f && timePercent < 0.75f) // Day time
        {
            RenderSettings.ambientLight = Color.white;
            RenderSettings.fogDensity = 0.01f;
        }
        else // Night time
        {
            RenderSettings.ambientLight = new Color(0.1f, 0.1f, 0.2f);
            RenderSettings.fogDensity = 0.05f; // Add thicker fog at night
        }
    }
}
