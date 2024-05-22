using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManagerFinal : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneController.instance.LoadScene("MainMenuScene");
        AudioManager.Instance.PlayMusic("Initial");
    }
}
