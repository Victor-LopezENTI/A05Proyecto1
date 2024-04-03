using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool paused;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void Settings()
    {

    }

    public void Quit()
    {
        
    }
}
