using UnityEngine;

public class SlingshotJump : MonoBehaviour
{
    private LineRenderer playerLR;
    private Rigidbody2D playerRB;

    [SerializeField] private float slingshotBuffer;
    private const float slingshotForce = 100f;
    private const int steps = 400;

    [SerializeField] private bool m_onSlingShot = false;
    public bool onSlingShot { get => m_onSlingShot; private set => m_onSlingShot = value; }
    [SerializeField] private bool m_chargingSlingshot = false;
    public bool chargingSlingshot { get => m_chargingSlingshot; private set => m_chargingSlingshot = value; }
    public bool startSlingshot { get; private set; } = false;
    public bool jumpingSlingshot { get; private set; } = false;

    private Vector2 dragStartPos;
    private Vector2[] trajectory;
    public Vector2 escapeForce { get; private set; }

    private void Awake()
    {
        playerLR = GetComponent<LineRenderer>();
        playerRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (onSlingShot)
        {
            if (InputManager.Instance.clickInput && !chargingSlingshot)
            {
                chargingSlingshot = true;
                dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (InputManager.Instance.clickInput && chargingSlingshot)
            {
                Vector2 dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                slingshotBuffer = (dragStartPos - dragEndPos).magnitude;
                slingshotBuffer = Mathf.Clamp(slingshotBuffer, 10f, 37f);
                playerLR.enabled = slingshotBuffer > 10f;

                escapeForce = (dragStartPos - dragEndPos).normalized * 2f * slingshotBuffer;
                float angle = Mathf.Atan2(dragStartPos.y - dragEndPos.y, dragStartPos.x - dragEndPos.x) * RotationManager.Instance.globalDirection.y;
                if (angle >= 0.5f && angle <= 2.5f)
                {
                    trajectory = new Vector2[steps];
                    trajectory = Plot(playerRB, playerRB.position, escapeForce, steps);

                    playerLR.positionCount = trajectory.Length;

                    Vector3[] positions = new Vector3[trajectory.Length];
                    for (int i = 0; i < trajectory.Length; i++)
                        positions[i] = trajectory[i];

                    playerLR.SetPositions(positions);
                }
            }
            else if (InputManager.Instance.clickReleased && chargingSlingshot)
            {
                startSlingshot = true;
                jumpingSlingshot = true;
                chargingSlingshot = false;
                playerLR.enabled = false;
                Vector2 dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                escapeForce = (dragStartPos - dragEndPos).normalized * slingshotBuffer * slingshotForce;
            }
            else
                startSlingshot = false;
        }
        else if (jumpingSlingshot)
        {
            jumpingSlingshot = !PlayerStateMachine.Instance.onGround;
        }
    }

    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        float distance = 0f;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;

            distance += moveStep.magnitude;

        }

        return results;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slingshot")
            onSlingShot = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slingshot")
            onSlingShot = false;
    }
}