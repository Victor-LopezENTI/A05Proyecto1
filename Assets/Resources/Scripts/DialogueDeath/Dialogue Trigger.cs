using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger: MonoBehaviour
{
    [SerializeField]public GameObject visualCue;
    private bool isPlayerInRange;

    [SerializeField] public TextAsset inkJSON;

    private void Awake()
    {
        isPlayerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !DialogueManager.GetInstance().dialoguePlaying)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.I))
            {
                DialogueManager.GetInstance().EnterDialogue(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D Trigger)
    {
        if (Trigger.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D Trigger)
    {
        if (Trigger.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
