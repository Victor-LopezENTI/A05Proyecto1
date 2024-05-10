using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool paused;
    private bool settingsON;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settingsON == false)
        {
            if (paused)
                ResumeGame();

            else
                PausedMenu();
        }
        if (InputManager.Instance.resetInput && !paused)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void PausedMenu()
    {
        pauseMenuUI.SetActive(true);
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        AudioManager.Instance.PlaySFX("Pause");
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
        Cursor.visible = false;
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
