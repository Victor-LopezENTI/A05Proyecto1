using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoulSpheresCollector : MonoBehaviour
{
    public int soulSphereCounter;
    public Text counterText;

    private void Start()
    {
        soulSphereCounter = PlayerPrefs.GetInt("SoulSphere", 0);
    }

    private void Update()
    {
        counterText.text = "" + soulSphereCounter.ToString();
    }
}