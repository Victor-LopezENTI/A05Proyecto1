using UnityEngine;

public class SlingshotJump : MonoBehaviour
{
    // Player components
    private LineRenderer playerLR;
    private Rigidbody2D playerRB;
    private BottomHooksBehaviour slingShot;

    // Slingshot tweak variables
    [SerializeField] public Vector2 escapeForce;
    [SerializeField] private Vector2 maxEscapeForce;
    [SerializeField] private float slingshotForce;

    // Slingshot states
    private bool onSlingShot;
    private bool onClick = false;
    public bool chargingSlingshot { get; private set; }
    public bool startSlingshot { get; private set; }
    public bool jumpingSlingshot { get; private set; }

    // Trajectory
    private Vector2 dragStartPos;
    private Vector2[] trajectory;
    private const int steps = 400;
    
    private void Awake()
    {
        playerLR = GetComponent<LineRenderer>();
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (onSlingShot)
        {
            switch (InputManager.Instance.clickInput)
            {
                case true when !onClick:
                    onClick = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                case true:
                {
                    Cursor.lockState = CursorLockMode.None;
                    Vector2 dragEndPos = -Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if (dragEndPos.magnitude < 5f)
                    {
                        playerLR.enabled = false;
                        chargingSlingshot = false;
                        return;
                    }
                
                    chargingSlingshot = true;
                    playerLR.enabled = true;
                    float angle = Mathf.Atan2(dragEndPos.y, dragEndPos.x) * RotationManager.instance.globalDirection.y;
                    if (angle is >= 0.5f and <= 2.5f)
                    {
                        slingShot.ChargeJumpAnimation(dragEndPos);

                        escapeForce = dragEndPos * slingshotForce;
                        Vector2 clampedEscapeForce = new Vector2(
                            Mathf.Clamp(escapeForce.x, -maxEscapeForce.x, maxEscapeForce.x),
                            Mathf.Clamp(escapeForce.y, -maxEscapeForce.y, maxEscapeForce.y));
                        escapeForce = clampedEscapeForce;

                        DrawTrajectory();
                    }
                    break;
                }
                default:
                {
                    if (InputManager.Instance.clickReleased && chargingSlingshot)
                    {
                        onClick = false;
                        slingShot.onTransition = false;
                        startSlingshot = true;
                        jumpingSlingshot = true;
                        chargingSlingshot = false;
                        playerLR.enabled = false;
                    }
                    else
                    {
                        startSlingshot = false;
                        escapeForce = Vector2.zero;
                    }
                    break;
                }
            }
        }
        else if (jumpingSlingshot)
        {
            onClick = false;
            jumpingSlingshot = !PlayerStateMachine.Instance.onGround;
        }
    }

    private void DrawTrajectory()
    {
        trajectory = new Vector2[steps];
        trajectory = Plot(playerRB, playerRB.position, escapeForce / 50f, steps);

        playerLR.positionCount = trajectory.Length;

        Vector3[] positions = new Vector3[trajectory.Length];
        for (int i = 0; i < trajectory.Length; i++)
            positions[i] = trajectory[i];

        playerLR.SetPositions(positions);
    }

    private static Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * (rigidbody.gravityScale * timestep * timestep);

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Slingshot")) return;
        onSlingShot = true;
        slingShot = collision.gameObject.GetComponent<BottomHooksBehaviour>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Slingshot")) return;
        onSlingShot = false;
        slingShot = null;
    }
}