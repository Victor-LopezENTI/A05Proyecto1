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
        Time.fixedDeltaTime = 0.0f;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.fixedDeltaTime = 1.0f;
        paused = false;
    }
    public void EnterSettings()
    {

    }

    public void ExitSettings()
    {
        settingsON = false;
        pauseMenuUI.SetActive(true);
        AudioManager.Instance.PlaySFX("ButtonClick");
    }

    public void Quit()
    {
        SceneController.instance.LoadScene("MainMenuScene");
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("Initial");
    }
}
