using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseAnim : MonoBehaviour
{
    [SerializeField] private GameObject mouseImage;
    private Dialogue dialogue;

    private void Start()
    {
        dialogue = GetComponentInParent<Dialogue>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(dialogue.dialoguePanel.activeSelf && (dialogue.index == 1 || dialogue.index == 2))
            {
                mouseImage.SetActive(true);
            }
            else
            {
                mouseImage.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mouseImage.SetActive(false);
        }
    }
}
