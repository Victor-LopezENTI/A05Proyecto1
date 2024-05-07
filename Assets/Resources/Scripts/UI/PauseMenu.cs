using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool paused;
    private bool settingsON;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settingsON == false)
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
        paused = true;
        AudioManager.Instance.PlaySFX("Pause");
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
    }
    public void EnterSettings()
    {

    }

    public void ExitSettings()
    {
        settingsON = false;
        pauseMenuUI.SetActive(true);
    }

    public void Quit()
    {
        SceneController.instance.LoadScene("MainMenuScene");
        AudioManager.Instance.PlayMusic("Initial");
    }
}
