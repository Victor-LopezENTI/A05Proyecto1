using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComp;
    public string[] lines;
    public float textSpeed;

    private int index;
    private bool near;
    public GameObject contButton;
    public GameObject dialoguePanel;

    private void Start()
    {
        textComp.text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && near == true)
        {
            if (textComp.text == lines[index])
            {
                NextLine();
            }

            else
            {
                StopAllCoroutines();
                textComp.text = lines[index];
            }
        }

        if (textComp.text == lines[index])
        {
            contButton.SetActive(true);
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    void EraseDialogue()
    {
        textComp.text = string.Empty;
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComp.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);

        if (index < lines.Length - 1)
        {
            index++;
            textComp.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            near = true;
            dialoguePanel.SetActive(true);
            StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            near = false;
            EraseDialogue();
            dialoguePanel.SetActive(false);
            StopAllCoroutines();
        }

    }
}
