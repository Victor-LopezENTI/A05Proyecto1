using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoulSpheresCollector : MonoBehaviour
{
    public static SoulSpheresCollector instance { get; private set; }
    public int soulSphereCounter;
    public int sceneSphereCounter;
    public Text counterText;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("There is already an instance of " + instance);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject.transform.root.gameObject);
        }
    }

    private void Start()
    {
        soulSphereCounter = 0;
        sceneSphereCounter = 0;
        counterText = GameUIManager.instance.sphereCollector;

    }

    private void Update()
    {
        if (counterText)
        {
            counterText.text = "" + soulSphereCounter.ToString();
        }
    }
}