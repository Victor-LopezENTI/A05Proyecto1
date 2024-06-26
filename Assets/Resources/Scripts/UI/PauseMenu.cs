using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject soulSpheres;
    private bool paused;
    private bool settingsON;
    public static PauseMenu instance { get; private set; }

    private void Awake()
    {
        if (instance)
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
        if (SceneManager.GetActiveScene().name != "MainMenuScene" && SceneManager.GetActiveScene().name != "ENDING CUTSCENE")
        {
            if (Input.GetKeyDown(KeyCode.Escape) && settingsON == false)
            {
                if (paused)
                    ResumeGame();

                else
                    PausedMenu();
            }
        }

        if (SceneManager.GetActiveScene().name == "PART 1 NEW")
        {
            soulSpheres.SetActive(true);
        }

        if (PlayerInput.instance && PlayerInput.instance.resetInput != 0f && !paused)
        {
            PlayerStateMachine.instance.isPaused = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SoulSpheresCollector.instance.soulSphereCounter -= SoulSpheresCollector.instance.sceneSphereCounter;
            SoulSpheresCollector.instance.sceneSphereCounter = 0;
        }

    }


    private void PausedMenu()
    {
        pauseMenuUI.SetActive(true);
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        AudioManager.Instance.PlaySFX("Pause");
        PlayerStateMachine.instance.isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        paused = false;
        PlayerStateMachine.instance.isPaused = false;
    }

    public void Quit()
    {
        SceneController.instance.LoadScene("MainMenuScene");
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic("Initial");
    }
}