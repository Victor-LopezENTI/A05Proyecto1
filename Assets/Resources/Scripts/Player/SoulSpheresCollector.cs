using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoulSpheresCollector : MonoBehaviour
{
    private int soulSphereCounter = 0;
    public Text counterText;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        PlayerPrefs.SetInt("SoulSphereCounter", soulSphereCounter);
        soulSphereCounter = PlayerPrefs.GetInt("SoulSphereCounter");
        counterText.text =  soulSphereCounter.ToString();
    }

    void OnTriggerEnter2D(Collider2D soulSphere)
    {
        if (soulSphere.gameObject.CompareTag("Soul")) 
        {
            soulSphereCounter++;
            PlayerPrefs.SetInt("SoulSphereCounter", soulSphereCounter);
            Debug.Log("Soul Sphere Collected. Counter: " + soulSphereCounter);
            Destroy(soulSphere.gameObject);
        }
    }

    private void Update()
    {
        counterText.text = "" + soulSphereCounter.ToString();
    }

}
