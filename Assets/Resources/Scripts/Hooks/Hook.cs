using System;
using Unity.VisualScripting;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private bool isSlingShot = false;
    private GameObject TopHook;
    private GameObject BottomHook;
    public static Action OnHookModeChanged;

    private void Awake()
    {
        GameManager.instance.AddHook(this);
        TopHook = transform.Find("TopHook").gameObject;
        BottomHook = transform.Find("BottomHook").gameObject;
    }

    private void FixedUpdate()
    {
        BottomHook.SetActive(isSlingShot);
        TopHook.SetActive(!isSlingShot);
    }

    public void ChangeState()
    {
        isSlingShot = !isSlingShot;
    }
}