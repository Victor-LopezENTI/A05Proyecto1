using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject.transform.root.gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void NextLevel()
    {
        if (SoulSpheresCollector.instance)
        {
            SoulSpheresCollector.instance.sceneSphereCounter = 0;
        }

        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadSceneAsync("MainMenuScene");
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        if (sceneName == "MainMenuScene")
        {
            Destroy(GameUIManager.instance.gameObject);
            Destroy(SoulSpheresCollector.instance.gameObject);
        }
        else
        {
            SoulSpheresCollector.instance.sceneSphereCounter = 0;
        }
    }
}
