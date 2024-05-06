using UnityEngine;

public class TopHooksBehaviour : MonoBehaviour
{
    [SerializeField] GameObject highlight;
    private void Start()
    {
        if (highlight == null)
            highlight = transform.GetChild(0).gameObject;
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
        }
    }

    public void setHilight(bool state)
    {
        highlight.SetActive(state);
    }
}