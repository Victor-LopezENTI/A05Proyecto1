using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuUIToolkit : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public GameObject settingsUI;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button resume = root.Q<Button>("ResumeButton");
        Button settings = root.Q<Button>("SettingsButton");
        Button quit = root.Q<Button>("QuitButton");

        resume.clicked += () => pauseMenu.ResumeGame();
        resume.clicked += () => gameObject.SetActive(false);
        resume.clicked += () => AudioManager.Instance.PlaySFX("ButtonClick");

        settings.clicked += () => pauseMenu.EnterSettings();
        settings.clicked += () => settingsUI.SetActive(true);
        settings.clicked += () => gameObject.SetActive(false);
        settings.clicked += () => AudioManager.Instance.PlaySFX("ButtonClick");

        quit.clicked += () => pauseMenu.Quit();
        quit.clicked += () => AudioManager.Instance.PlaySFX("ButtonClick");
    }

}
