using UnityEngine;

public class Jumping : IPlayerState
{
    public void OnEnter()
    {
    }

    public void Update()
    {
        if (PlayerStateMachine.instance.playerRb.velocity.y > 0)
        {
            PlayerStateMachine.instance.playerAnimator.Play("jump");
        }
        else
        {
            PlayerStateMachine.instance.playerAnimator.Play("fall");
        }
    }

    public void FixedUpdate()
    {
    }

    public void OnExit()
    {
    }
}
