using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject[] choices;
    private Text[] choicesText;

    private Story currentStory;

    public bool dialoguePlaying { get; private set; }

    private static DialogueManager instance;

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

        choicesText = new Text[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<Text>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialoguePlaying)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ContinueStory();
        }

        if(SoulSpheresCollector.instance.soulSphereCounter == 7)
        {
            currentStory.variablesState["allCollectiblesGathered"] = true;
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
        PlayerStateMachine.instance.isPaused = true;

        ContinueStory();
    }

    private IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.2f);

        dialoguePlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        PlayerStateMachine.instance.isPaused = false;
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
        }

        for (int i=index; i<choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        SelectFirstChoice();
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoiceBasedOnBool()
    {
        bool choiceBool = (bool)currentStory.variablesState["allCollectiblesGathered"];
        int choiceIndex = choiceBool ? 0 : 1;

        if (choiceIndex < currentStory.currentChoices.Count)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
        else
        {
            Debug.LogError("Choice index out of bounds");
        }
    }
}
