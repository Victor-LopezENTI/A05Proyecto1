using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    private UIDocument doc;
    private List<Button> buttons = new List<Button>();

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        buttons = doc.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnDisable()
    {
        for (int i = 0;i < buttons.Count;i++)
        {
            buttons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnAllButtonsClick(ClickEvent evnt)
    {
        gameObject.SetActive(false);
    }
}
