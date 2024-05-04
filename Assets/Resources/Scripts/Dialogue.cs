using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public Text txt;
    public string[] lines;
    public float textSpeed;

    private int index;
    private bool near;
    private bool alreadyEntered;
    public GameObject contButton;
    public GameObject dialoguePanel;
    public List<string> importantTxt;

    private void Start()
    {
        txt.text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && near == true)
        {
            NextLine();
            
        }

        if (txt.text == lines[index])
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
        txt.text = string.Empty;
    }

    IEnumerator TypeLine()
    {
        string line = lines[index];
        string partialLine = "";

        for (int i = 0; i < line.Length; i++)
        {
            partialLine += line[i];

            foreach (string palabra in importantTxt)
            {
                if (partialLine.EndsWith(palabra))
                {
                    partialLine = partialLine.Substring(0, partialLine.Length - palabra.Length) + "<color=red>" + palabra + "</color>";
                }
            }

            txt.text = partialLine;
            yield return new WaitForSeconds(textSpeed); 
        }

        foreach (string palabra in importantTxt)
        {
            if (partialLine.EndsWith(palabra))
            {
                partialLine += "</color>";
                txt.text = partialLine;
                break;
            }
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);

        if (index < lines.Length - 1)
        {
            index++;
            txt.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !alreadyEntered)
        {
            near = true;
            dialoguePanel.SetActive(true);
            StartDialogue();
            alreadyEntered = true;
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
