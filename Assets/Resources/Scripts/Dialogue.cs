using System.Collections;
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

    private void Start()
    {
        txt.text = string.Empty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && near == true)
        {
            if (txt.text == lines[index])
            {
                NextLine();
            }

            else
            {
                StopAllCoroutines();
                txt.text = lines[index];
            }
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
        foreach (char c in lines[index].ToCharArray())
        {
            txt.text += c;
            yield return new WaitForSeconds(textSpeed);
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
