using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HookLightBehaviour : MonoBehaviour
{
    private new Light2D light;
    private bool lightChanged;
    
    private void Awake()
    {
        light = GetComponent<Light2D>();
    }

    private void SetLight()
    {
        if (lightChanged)
        {
            light.intensity = 0;
            lightChanged = false;
        }
        else
        {
            light.intensity = 100;
            lightChanged = true;
        }
    }
}