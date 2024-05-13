using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roping : IPlayerState
{
    private const float RopeExpansionSpeed = 0.15f;
    private const float ClimbSpeed = 0.15f;
    
    private HingeJoint2D _hingeJoint = PlayerStateMachine.instance.GetComponent<HingeJoint2D>();
    private GameObject _ropePrefab = Resources.Load<GameObject>("Prefabs/Player/Rope");
    private GameObject _selectedHook;

    public void OnEnter()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += ctx => PlayerStateMachine.instance.horizontalInput = ctx.ReadValue<float>();
        InputManager.PlayerInputActions.Player.VerticalMovement.performed += ctx => PlayerStateMachine.instance.jumpInput = ctx.ReadValue<float>();
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
    }

    public void OnExit()
    {
    }
}
