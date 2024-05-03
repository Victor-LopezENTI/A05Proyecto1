using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenLogic : MonoBehaviour
{
    public Toggle fullScreen;

    void Start()
    {
        if (Screen.fullScreen)
            fullScreen.isOn = true;

        else
            fullScreen.isOn = false;
    }

    public void SetFullScreen(bool screen)
    {
        Screen.fullScreen = screen;
    }
}
