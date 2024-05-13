using System.Collections;
using UnityEngine;

public class Roping : IPlayerState
{
    private const float RopeExpansionSpeed = 0.15f;
    private const float ClimbSpeed = 0.15f;

    private static MonoBehaviour _monoBehaviour;
    private readonly GameObject _ropePrefab = Resources.Load<GameObject>("Prefabs/Player/Rope");
    private LineRenderer _ropeLineRenderer;
    private HingeJoint2D _hingeJoint;

    private float _selectedHookDistance;
    private float _selectedHookAngle;

    private bool _hingeConnected;
    private Vector3 _savedPos;

    public Roping(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
    }

    public void OnEnter()
    {
        InputManager.PlayerInputActions.Player.HorizontalMovement.performed += ctx =>
            PlayerStateMachine.instance.horizontalInput = ctx.ReadValue<float>();
        InputManager.PlayerInputActions.Player.VerticalMovement.performed +=
            ctx => PlayerStateMachine.instance.jumpInput = ctx.ReadValue<float>();
    }

    public void Update()
    {
        _ropeLineRenderer.SetPosition(0, PlayerStateMachine.instance.playerRb.transform.position);

        _selectedHookAngle = Vector2.SignedAngle(Vector2.right,
            PlayerStateMachine.instance.selectedHook.transform.position -
            PlayerStateMachine.instance.playerRb.transform.position);

        _selectedHookDistance = Vector2.Distance(PlayerStateMachine.instance.playerRb.transform.position,
            PlayerStateMachine.instance.transform.position);
    }

    public void FixedUpdate()
    {
    }

    private void LaunchRope(Transform hook)
    {
        if (RopeHasTrajectory(hook))
        {
            _ropeLineRenderer = Object.Instantiate(_ropePrefab, PlayerStateMachine.instance.playerRb.transform)
                .GetComponent<LineRenderer>();
            _ropeLineRenderer.SetPosition(0, PlayerStateMachine.instance.playerRb.transform.position);
            _ropeLineRenderer.SetPosition(1, PlayerStateMachine.instance.playerRb.transform.position);
            _monoBehaviour.StartCoroutine(ExpandLine(hook));
        }
    }

    private bool RopeHasTrajectory(Transform hook)
    {
        return !Physics2D.Linecast(PlayerStateMachine.instance.playerRb.transform.position, hook.position,
            PlayerStateMachine.instance.groundLayer);
    }

    public void CompareHook(GameObject hook)
    {
        if (RopeHasTrajectory(hook.transform))
        {
            if (PlayerStateMachine.instance.selectedHook == null)
            {
                PlayerStateMachine.instance.selectedHook = hook;
                hook.GetComponent<TopHooksBehaviour>().SetHilight(true);
            }
            else if (_ropeLineRenderer == null &&
                     Vector2.Distance(PlayerStateMachine.instance.playerRb.transform.position,
                         hook.transform.position) < Vector2.Distance(
                         PlayerStateMachine.instance.playerRb.transform.position,
                         PlayerStateMachine.instance.selectedHook.transform.position))
            {
                PlayerStateMachine.instance.selectedHook.GetComponent<TopHooksBehaviour>().SetHilight(false);
                PlayerStateMachine.instance.selectedHook = null;
                PlayerStateMachine.instance.selectedHook = hook;
                hook.GetComponent<TopHooksBehaviour>().SetHilight(true);
            }
        }
        else
        {
            if (PlayerStateMachine.instance.selectedHook == hook)
            {
                DeselectHook();
            }
        }
    }

    void HookIsConnected()
    {
        _hingeConnected = true;
        _savedPos = PlayerStateMachine.instance.selectedHook.transform.position;
        _hingeJoint.connectedBody = PlayerStateMachine.instance.selectedHook.GetComponent<Rigidbody2D>();
        _hingeJoint.enabled = true;
    }
    
    public void CheckExittingHook(GameObject hook)
    {
        if (PlayerStateMachine.instance.selectedHook == hook)
        {
            DeselectHook();
        }
    }

    void DeselectHook()
    {
        if (_ropeLineRenderer != null)
        {
            DestroyRope();
        }
        selectedHook.GetComponent<TopHooksBehaviour>().SetHilight(false);
        selectedHook = null;
    }

    void HookIsConnected()
    {
        hingeConnected = true;
        savedPos = selectedHook.transform.position;
        hjoint.connectedBody = selectedHook.GetComponent<Rigidbody2D>();
        hjoint.enabled = true;
    }
    public bool ClimbRope(float v)
    {
        Vector2 tempPos = Vector2.MoveTowards(playerRB.position, selectedHook.transform.position, v * climbSpeed);
        float tempDist = Vector2.Distance(tempPos, selectedHook.transform.position);
        if (!((v > 0 && tempDist < 4) || (v < 0 && tempDist > selectedHook.GetComponent<CircleCollider2D>().radius * selectedHook.transform.lossyScale.x)))
        {
            hjoint.connectedBody = null;
            playerRB.position = tempPos;
            hjoint.connectedBody = selectedHook.GetComponent<Rigidbody2D>();
            return true;
        }
        return false;
    }
    void DestroyRope()
    {
        selectedHook.GetComponent<Rigidbody2D>().angularVelocity = 0;
        hjoint.enabled = false;
        hjoint.connectedBody = null;
        Destroy(_ropeLineRenderer.gameObject);
        hingeConnected = false;
    }

    private IEnumerator ExpandLine(Transform hook)
    {
        for (float i = 0; i < 1; i += Time.deltaTime * RopeExpansionSpeed)
        {
            if (_ropeLineRenderer != null)
            {
                _ropeLineRenderer.SetPosition(0, PlayerStateMachine.instance.playerRb.transform.position);
                _ropeLineRenderer.SetPosition(1,
                    Vector3.Lerp(PlayerStateMachine.instance.playerRb.transform.position, hook.position, i));
                yield return null;
            }
            else
                yield break;
        }

        if (_ropeLineRenderer != null)
        {
            _ropeLineRenderer.SetPosition(1, hook.position);
            HookIsConnected();
        }

        yield break;
    }

    public void OnExit()
    {
    }
}