using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    public Animator button;
    void Awake()
    {
        button.SetBool("pressed", false);  
    }

    // Update is called once per frame
    public void ButtonClicked()
    {
        button.SetBool("pressed", true);
    }
}
