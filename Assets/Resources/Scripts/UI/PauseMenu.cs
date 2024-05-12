using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool paused;
    private bool settingsON;
    public static PauseMenu instance { get; private set; }

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
        pauseMenuUI = GameUIManager.instance.pauseUI;
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        /*
         * if (Input.GetKeyDown(KeyCode.Escape) && settingsON == false)
        {
            if (paused)
                ResumeGame();

            else
                PausedMenu();
        }
        if (InputManager.instance.resetInput && !paused)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SoulSpheresCollector.instance.soulSphereCounter -= SoulSpheresCollector.instance.sceneSphereCounter;
            SoulSpheresCollector.instance.sceneSphereCounter = 0;
        }
         */
    }
    

    private void PausedMenu()
    {
        pauseMenuUI.SetActive(true);
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        AudioManager.Instance.PlaySFX("Pause");
      //  PlayerStateMachine.instance.isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        paused = false;
        Cursor.visible = false;
       // PlayerStateMachine.instance.isPaused = false;
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
