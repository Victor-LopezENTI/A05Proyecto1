using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Hook : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Light2D _light2D;
    [SerializeField] private bool isSlingShot;
    private GameObject _topHook;
    private GameObject _bottomHook;

    private void Awake()
    {
        GameManager.instance.AddHook(this);
        _topHook = transform.Find("TopHook").gameObject;
        _bottomHook = transform.Find("BottomHook").gameObject;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light2D = GetComponent<Light2D>();

        if (isSlingShot)
        {
            _spriteRenderer.color = new Color(0.8f, 0.2f, 0.8f, 1f);
            _light2D.color = new Color(0.5f, 0f, 0.2f, 1f);
            _light2D.intensity = 5f;
        }
        else
        {
            _spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        _bottomHook.SetActive(isSlingShot);
        _topHook.SetActive(!isSlingShot);
    }

    public void ChangeState()
    {
        isSlingShot = !isSlingShot;
    }
}