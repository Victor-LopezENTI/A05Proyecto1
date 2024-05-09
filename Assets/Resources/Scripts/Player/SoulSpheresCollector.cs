using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulSpheresCollector : MonoBehaviour
{
    public int soulSphereCounter = 0;
    public TMP_Text counterText;
   
    void Start()
    {
        soulSphereCounter = PlayerPrefs.GetInt("SoulSphereCounter", 0);
        counterText.text =  soulSphereCounter.ToString();
    }

    void OnTriggerEnter2D(Collider2D soulSphere)
    {
        if (soulSphere.gameObject.CompareTag("Soul")) 
        {
            PlayerPrefs.SetInt("SoulSphereCounter", soulSphereCounter);
            Debug.Log("Soul Sphere Collected. Counter: " + ++soulSphereCounter);
            Destroy(soulSphere.gameObject);
            counterText.text = soulSphereCounter.ToString();
        }
    }

}
