using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject[] choices;
    private Text[] choicesText;

    private Story currentStory;

    public bool dialoguePlaying { get; private set; }
    private bool makingChoice;
    public bool goodEnding;
    private int spheres;

    public static DialogueManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Already a Dialogue manager");
        }
        instance = this;
    }

    private void Start()
    {
        dialoguePlaying = false;
        dialoguePanel.SetActive(false);
        makingChoice = false;

        choicesText = new Text[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<Text>();
            index++;
        }

        spheres = SoulSpheresCollector.instance.soulSphereCounter;
    }

    private void Update()
    {
        if (!dialoguePlaying)
            return;

        if (Input.GetKeyDown(KeyCode.F) && makingChoice == true)
        {
            ContinueStory();
        }

        if (spheres == 7)
        {
            currentStory.variablesState["allCollectiblesGathered"] = true;
            goodEnding = true;
        }

        if (spheres != 7)
        {
            currentStory.variablesState["allCollectiblesGathered"] = false;
            goodEnding = false;
        }

        
    }

    public static DialogueManager GetInstance()
    {
       return instance;
    }

    public void EnterDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialoguePlaying = true;
        dialoguePanel.SetActive(true);
        makingChoice = true;
        PlayerStateMachine.instance.isPaused = true;

        ContinueStory();
    }

    private IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.2f);

        dialoguePlaying = false;
        dialoguePanel.SetActive(false);
        makingChoice = false;
        dialogueText.text = "";
        PlayerStateMachine.instance.isPaused = false;

        if (goodEnding == false)
        {
            SceneManager.LoadScene("ENDING CUTSCENE");
        }
        else
        {
            SceneManager.LoadScene("FinalCredits");
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic("FinalCredits");
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }

        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currenthcoices = currentStory.currentChoices;

        if(currenthcoices.Count > choices.Length)
        {
            Debug.LogError("Too many choices");
        }

        int index = 0;
        foreach(Choice choice in currenthcoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
            makingChoice = false;
        }

        for (int i=index; i<choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
    }

    public void MakeChoiceBasedOnBool()
    {
        bool choiceBool = (bool)currentStory.variablesState["allCollectiblesGathered"];
        int choiceIndex = choiceBool ? 0 : 1;
        
        currentStory.ChooseChoiceIndex(choiceIndex);
        AudioManager.Instance.PlaySFX("ButtonClick");
        makingChoice = true;
        ContinueStory();
    }
}
