using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject mainMenu;

    private UIDocument doc;
    private List<Button> buttons = new List<Button>();
    private Button start;
    private Button settings;
    private Button credits;
    private Button quit;
    private VisualElement sureQuit;
    private Button yesQuit;
    private Button noQuit;

    private void Update()
    {
        doc = GetComponent<UIDocument>();
        start = doc.rootVisualElement.Q("StartButton") as Button;
        settings = doc.rootVisualElement.Q("SettingsButton") as Button;
        credits = doc.rootVisualElement.Q("CreditsButton") as Button;
        quit = doc.rootVisualElement.Q("QuitButton") as Button;
        yesQuit = doc.rootVisualElement.Q("YesQuit") as Button;
        noQuit = doc.rootVisualElement.Q("NoQuit") as Button;

        start.RegisterCallback<ClickEvent>(OnStartButtonsClick);
        settings.RegisterCallback<ClickEvent>(OnSettingsButtonsClick);
        credits.RegisterCallback<ClickEvent>(OnCreditsButtonsClick);
        quit.RegisterCallback<ClickEvent>(OnQuitButtonsClick);

        sureQuit = doc.rootVisualElement.Q("SureQuit") as VisualElement;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].RegisterCallback<ClickEvent>(ActivateMainMenu);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].UnregisterCallback<ClickEvent>(ActivateMainMenu);
        }
    }

    private void ActivateMainMenu(ClickEvent evnt)
    {
        mainMenu.SetActive(true);
    }

    private void OnStartButtonsClick(ClickEvent evnt)
    {
        SceneController.instance.NextLevel();
    }
    private void OnSettingsButtonsClick(ClickEvent evnt)
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    private void OnCreditsButtonsClick(ClickEvent evnt)
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }
    private void OnQuitButtonsClick(ClickEvent evnt)
    {
        sureQuit.style.display = DisplayStyle.Flex;
        yesQuit.RegisterCallback<ClickEvent>(OnYesQuitButtonsClick);
        noQuit.RegisterCallback<ClickEvent>(OnNoQuitButtonsClick);
    }

    private void OnYesQuitButtonsClick(ClickEvent evnt)
    {
        Application.Quit();
    }

    private void OnNoQuitButtonsClick(ClickEvent evnt)
    {
        sureQuit.style.display = DisplayStyle.None;
    }
}
