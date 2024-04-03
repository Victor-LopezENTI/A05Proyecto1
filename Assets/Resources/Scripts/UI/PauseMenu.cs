using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool paused;
<<<<<<< HEAD:Assets/Resources/Scripts/UI/PauseMenu.cs
=======
    private bool settingsON;
>>>>>>> feature/victor:Assets/Scripts/UI/PauseMenu.cs

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
<<<<<<< HEAD:Assets/Resources/Scripts/UI/PauseMenu.cs
        if (Input.GetKeyDown(KeyCode.Escape))
=======
        if (Input.GetKeyDown(KeyCode.Escape) && settingsON == false)
>>>>>>> feature/victor:Assets/Scripts/UI/PauseMenu.cs
        {
            if (paused)
                ResumeGame();

            else
                PausedMenu();
        }
    }

    public void PausedMenu()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        paused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }

<<<<<<< HEAD:Assets/Resources/Scripts/UI/PauseMenu.cs
    public void Settings()
    {

=======
    public void EnterSettings()
    {
        settingsON = true;
        paused = true;
    }

    public void ExitSettings()
    {
        settingsON = false;
        paused = false;
>>>>>>> feature/victor:Assets/Scripts/UI/PauseMenu.cs
    }

    public void Quit()
    {
<<<<<<< HEAD:Assets/Resources/Scripts/UI/PauseMenu.cs
        
=======
        Application.Quit();
        Debug.Log("EXIT");
>>>>>>> feature/victor:Assets/Scripts/UI/PauseMenu.cs
    }
}
