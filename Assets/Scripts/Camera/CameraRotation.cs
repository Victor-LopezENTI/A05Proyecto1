using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // The animated target containing the camera states
    private Animator animator;

    public bool onTransition { get; private set; } = false;

    // Buffer for calculating the chamber rotation time (not delta-time based)
    [SerializeField] private float transitionBuffer = 0f;
    public float maxTransitionBuffer { get; private set; } = 2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (onTransition)
        {
            transitionBuffer += Time.unscaledDeltaTime;
            if (transitionBuffer >= maxTransitionBuffer)
            {
                onTransition = false;
                transitionBuffer = 0f;
            }
        }
    }

    public void transitionCamera()
    {
        if (RotationManager.Instance.GetChamberUpsideDown())
        {
            animator.Play("Upside Down");
        }
        else
        {
            animator.Play("Upside Up");
        }

        // Start the transition buffer
        transitionBuffer = 0f;
        onTransition = true;
    }

    public bool IsOnTransition() { return onTransition; }
}