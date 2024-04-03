using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD:Assets/Resources/Scripts/UI/Settings/VolumeLogic.cs

public class VolumeLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
=======
using UnityEngine.UI;

public class VolumeLogic : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image music;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        AudioListener.volume = slider.value;
        CheckMute();
    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("Volume", sliderValue);
        AudioListener.volume = slider.value;
        CheckMute();
    }

    public void CheckMute()
    {
        if (sliderValue == 0)
            music.enabled = true;

        else
            music.enabled = false;
>>>>>>> feature/victor:Assets/Scripts/UI/Settings/VolumeLogic.cs
    }
}
