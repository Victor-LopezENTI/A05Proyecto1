using System;
using System.Collections;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    
    public static RopeManager Instance;
    
    private Rigidbody2D _playerRb;
    private LayerMask _groundLayer;
    [SerializeField] private GameObject ropePrefab; 
    private LineRenderer _ropeLineRenderer; 
    
    public GameObject selectedHook { get; private set; }
    public float selectedHookAngle { get; private set; }
    public float selectedHookDistance { get; private set; }
    private float ropeSpeed = 40;

    public bool hingeConnected = false;
    public bool enteringRope { get; private set; } = false;
    public bool leavingRope { get; private set; } = false;
    [SerializeField] float ropeExpansionSpeed;
    [SerializeField] HingeJoint2D hjoint;

    private Vector3 savedPos;
    private const float climbSpeed = 0.15f;

    private void Awake()
    {
        #region Singleton Pattern

        if (Instance)
        {
            Debug.Log("There is already an instance of " + Instance);
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        #endregion
        
        _playerRb = GetComponent<Rigidbody2D>();
        _groundLayer = LayerMask.GetMask("Platforms");
    }

    private void Update()
    {
        if (_ropeLineRenderer)
        {
            _ropeLineRenderer.SetPosition(0, _playerRb.transform.position);
            selectedHookAngle = Vector2.SignedAngle(Vector2.right, selectedHook.transform.position - _playerRb.transform.position);
            selectedHookDistance = Vector2.Distance(_playerRb.transform.position, selectedHook.transform.position);
        }
    }
    private void FixedUpdate()
    {
        if (selectedHook != null && !playerSM.onGround && InputManager.instance.clickInput && _ropeLineRenderer == null)
        {
            LaunchRope(selectedHook.transform);
            enteringRope = true;
        }
        else
            enteringRope = false;

        if (hingeConnected)
        {
            selectedHook.GetComponent<Rigidbody2D>().position = savedPos;
            if (InputManager.instance.moveInput != 0 &&
            ((InputManager.instance.moveInput < 0 && transform.position.x < selectedHook.transform.position.x) ||
            (InputManager.instance.moveInput > 0 && transform.position.x < selectedHook.transform.position.x)))
            {
                playerRB.AddForce(new Vector2(InputManager.instance.moveInput * ropeSpeed, 0));
            }
            playerRB.velocity = Vector2.ClampMagnitude(playerRB.velocity, 100);
        }

        if (_ropeLineRenderer != null && (!InputManager.instance.clickInput || playerSM.onGround))
        {
            DestroyRope();
            leavingRope = true;
            selectedHook.GetComponent<Rigidbody2D>().angularVelocity = 0f;
            selectedHook.GetComponent<Rigidbody2D>().rotation = 0f;
        }
        else
            leavingRope = false;
    }

    private void LaunchRope(Transform hook)
    {
        if (RopeHasTrajectory(hook))
        {
            _ropeLineRenderer = Instantiate(ropePrefab, playerRB.transform).GetComponent<LineRenderer>();
            _ropeLineRenderer.SetPosition(0, playerRB.transform.position);
            _ropeLineRenderer.SetPosition(1, playerRB.transform.position);
            StartCoroutine(ExpandLine(hook));
        }
    }

    private bool RopeHasTrajectory(Transform hook)
    {
        return !Physics2D.Linecast(playerRB.transform.position, hook.position, obstacleLayers);
    }
    public void CompareHook(GameObject hook)
    {
        if (RopeHasTrajectory(hook.transform))
        {
            if (selectedHook == null)
            {
                selectedHook = hook;
                hook.GetComponent<TopHooksBehaviour>().SetHilight(true);
            }
            else if (_ropeLineRenderer == null && Vector2.Distance(playerRB.transform.position, hook.transform.position) < Vector2.Distance(playerRB.transform.position, selectedHook.transform.position))
            {
                selectedHook.GetComponent<TopHooksBehaviour>().SetHilight(false);
                selectedHook = null;
                selectedHook = hook;
                hook.GetComponent<TopHooksBehaviour>().SetHilight(true);
            }
        }
        else
        {
            if (selectedHook == hook)
            {
                DeselectHook();
            }
        }
    }

    public void CheckExittingHook(GameObject hook)
    {
        if (selectedHook == hook)
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

    IEnumerator ExpandLine(Transform hook)
    {
        for (float i = 0; i < 1; i += Time.deltaTime * ropeExpansionSpeed)
        {
            if (_ropeLineRenderer != null)
            {
                _ropeLineRenderer.SetPosition(0, playerRB.transform.position);
                _ropeLineRenderer.SetPosition(1, Vector3.Lerp(playerRB.transform.position, hook.position, i));
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
}