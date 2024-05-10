using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HookLightBehaviour : MonoBehaviour
{
    private Light2D light;
    private bool lightChanged;
    
    private void Awake()
    {
        light = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        TopHooksBehaviour.OnHookSelected += SetLight;
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

    private void OnDisable()
    {
        TopHooksBehaviour.OnHookSelected -= SetLight;
    }
}