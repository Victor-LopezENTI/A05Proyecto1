using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // The animated target containing the camera states
    private Animator animator;

    private bool onTransition = false;

    // Buffer for calculating the chamber rotation time (not delta-time based)
    private float transitionBuffer = 0f;
    private const float maxTransitionBuffer = 2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void transitionCamera()
    {
        if (RotationManager.Instance.getChamberUpsideDown())
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
}