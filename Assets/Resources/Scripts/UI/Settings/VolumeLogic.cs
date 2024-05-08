using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeLogic : MonoBehaviour
{
    public Slider MusicSlider;
    public Slider SFXSlider;
    public float sliderValue;
    public Image VolumeMusic;
    public Image SFXMusic;

    private void Start()
    {
        SFXMusic.enabled = false;
        VolumeMusic.enabled = false;
    }

    public void ChangeVolume()
    {
        AudioManager.Instance.MusicVolume(MusicSlider.value);
        CheckMute();
    }

    public void ChangeSFX()
    {
        AudioManager.Instance.SFXVolume(SFXSlider.value);
        CheckMute();
    }

    public void CheckMute()
    {
        if (MusicSlider.value == 0)
            VolumeMusic.enabled = true;

        else if(MusicSlider.value != 0)
            VolumeMusic.enabled = false;

        if (SFXSlider.value == 0)
            SFXMusic.enabled = true;

        else if (SFXSlider.value != 0)
            SFXMusic.enabled = false;
    }
}
