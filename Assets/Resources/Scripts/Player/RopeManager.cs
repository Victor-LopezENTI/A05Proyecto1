using System.Collections;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    public static RopeManager Instance;

    // Constants
    private const float RopeExpansionSpeed = 7f;
    private const float ClimbSpeed = 0.15f;

    // Variables
    public GameObject selectedHook;
    public LineRenderer ropeLineRenderer;
    [SerializeField] public ParticleSystem sparks;

    [SerializeField] private GameObject ropePrefab;
    [SerializeField] private HingeJoint2D _hingeJoint;
    private Rigidbody2D _playerRb;
    private LayerMask _obstacleLayer;

    public float selectedHookAngle { get; private set; }
    public float selectedHookDistance { get; private set; }

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
        _obstacleLayer = LayerMask.GetMask("Platforms");
    }

    private void OnEnable()
    {
        ropeLineRenderer = null;
    }

    public void LaunchRope(Transform hook)
    {
        if (RopeHasTrajectory(hook))
        {
            ropeLineRenderer = Instantiate(ropePrefab, _playerRb.transform).GetComponent<LineRenderer>();
            ropeLineRenderer.SetPosition(0, _playerRb.transform.position);
            ropeLineRenderer.SetPosition(1, _playerRb.transform.position);
            StartCoroutine(ExpandLine(hook));
        }
    }

    public void DeselectHook()
    {
        if (ropeLineRenderer)
        { 
            DestroyRope();
        }
        
        selectedHook.GetComponent<TopHooksBehaviour>().SetHilight(false);
        Instance.selectedHook.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        Instance.selectedHook.GetComponent<Rigidbody2D>().rotation = 0f;
        selectedHook = null;
    }

    private bool RopeHasTrajectory(Transform hook)
    {
        return !Physics2D.Linecast(_playerRb.transform.position, hook.position, _obstacleLayer);
    }

    public void CompareHook(GameObject hook)
    {
        if (RopeHasTrajectory(hook.transform))
        {
            if (!selectedHook)
            {
                selectedHook = hook;
                hook.GetComponent<TopHooksBehaviour>().SetHilight(true);
            }
            else if (ropeLineRenderer == null &&
                     Vector2.Distance(_playerRb.transform.position, hook.transform.position) <
                     Vector2.Distance(_playerRb.transform.position, selectedHook.transform.position))
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

    private void AssignHinge()
    {
        _hingeJoint.connectedBody = selectedHook.GetComponent<Rigidbody2D>();
        _hingeJoint.enabled = true;
    }

    public void ClimbRope(float v)
    {
        Vector2 tempPos = Vector2.MoveTowards(_playerRb.position, selectedHook.transform.position, v * ClimbSpeed);
        float tempDist = Vector2.Distance(tempPos, selectedHook.transform.position);
        if (!((v > 0 && tempDist < 4) || (v < 0 && tempDist >
                selectedHook.GetComponent<CircleCollider2D>().radius * selectedHook.transform.lossyScale.x)))
        {
            _hingeJoint.connectedBody = null;
            _playerRb.position = tempPos;
            _hingeJoint.connectedBody = selectedHook.GetComponent<Rigidbody2D>();
        }
    }

    private void DestroyRope()
    {
        _hingeJoint.enabled = false;
        _hingeJoint.connectedBody = null;
        Destroy(ropeLineRenderer.gameObject);
    }

    private IEnumerator ExpandLine(Transform hook)
    {
        for (float i = 0; i < 1; i += Time.deltaTime * RopeExpansionSpeed)
        {
            if (ropeLineRenderer)
            {
                ropeLineRenderer.SetPosition(0, _playerRb.transform.position);
                ropeLineRenderer.SetPosition(1, Vector3.Lerp(_playerRb.transform.position, hook.position, i));
                yield return null;
            }
            else
                yield break;
        }

        if (!ropeLineRenderer) yield break;

        ropeLineRenderer.SetPosition(1, hook.position);
        AssignHinge();
    }
}