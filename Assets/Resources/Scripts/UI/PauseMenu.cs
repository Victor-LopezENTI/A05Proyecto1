using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
<<<<<<< HEAD
    private bool paused;
=======
    private bool paused;
    private bool settingsON;
>>>>>>> origin/feature/victor

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
<<<<<<< HEAD
    {
        if (Input.GetKeyDown(KeyCode.Escape))
=======
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settingsON == false)
>>>>>>> origin/feature/victor
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
<<<<<<< HEAD
    }

    public void Settings()
    {

    }

    public void Quit()
    {
        
=======
    }
    public void EnterSettings()
    {
        settingsON = true;
        paused = true;
    }

    public void ExitSettings()
    {
        settingsON = false;
        paused = false;
    }

    public void Quit()
    {
        Application.Quit();
>>>>>>> origin/feature/victor
    }
}
