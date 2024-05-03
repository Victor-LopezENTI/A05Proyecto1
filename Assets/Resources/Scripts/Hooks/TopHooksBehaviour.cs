using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class TopHooksBehaviour : MonoBehaviour
{
    [SerializeField] GameObject highlight;
    private void Start()
    {
        if(highlight == null)
            highlight = transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<RopeManager>().compareHook(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<RopeManager>().checkExittingHook(this.gameObject);
        }
    }

    public void setHilight(bool state)
    {
        highlight.SetActive(state);
    }
}