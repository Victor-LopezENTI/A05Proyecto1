using System;
using UnityEngine;

public class TopHooksBehaviour : MonoBehaviour
{
    [SerializeField] GameObject highlight;
    public static Action OnHookSelected;
    
    private void Start()
    {
        if (highlight == null)
            highlight = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<RopeManager>().CompareHook(this.gameObject);
            OnHookSelected?.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<RopeManager>().CompareHook(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<RopeManager>().CheckExittingHook(this.gameObject);
            OnHookSelected?.Invoke();
        }
    }

    public void SetHilight(bool state)
    {
        highlight.SetActive(state);
    }
}