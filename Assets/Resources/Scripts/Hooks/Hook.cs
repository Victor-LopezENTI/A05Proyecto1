using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private bool isSlingShot = false;
    GameObject TopHook;
    TopHooksBehaviour TopHooksBehaviour;
    GameObject BottomHook;

    private void Awake()
    {
        GameManager.Instance.AddHook(this);
        TopHook = transform.Find("TopHook").gameObject;
        BottomHook = transform.Find("BottomHook").gameObject;
        TopHooksBehaviour = GetComponent<TopHooksBehaviour>();
    }

    private void FixedUpdate()
    {
        BottomHook.SetActive(isSlingShot);
        TopHook.SetActive(!isSlingShot);
    }

    public void changeState()
    {
        isSlingShot = !isSlingShot;
    }
}