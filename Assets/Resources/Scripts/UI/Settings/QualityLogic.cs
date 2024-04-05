using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualityLogic : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int quality;

    void Start()
    {
        quality = PlayerPrefs.GetInt("Quality");
        dropdown.value = quality;
        ChangeQuality();
    }

    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("Quality", dropdown.value);
        quality = dropdown.value;
    }
}
