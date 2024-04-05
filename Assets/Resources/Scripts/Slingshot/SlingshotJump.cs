using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotJump : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;

    private Rigidbody2D playerRB;

    [SerializeField] private float slingshotForce = 15f;
    [SerializeField] private int steps = 500;

    PlayerStateMachine playerStateMachine;

    private bool jumpInput;
    [SerializeField] private float slingShotBuffer = 0f;
    [SerializeField] private const float maxSlingShotBuffer = 1f;

    [SerializeField] public bool onSlingShot { get; private set; } = false;
    [SerializeField] private bool chargingSlingshot = false;

    private Vector2 startPos;
    private Vector2 endPos;
    // Final velocity
    public Vector2 velocity { get; private set; }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        playerStateMachine = PlayerStateMachine.Instance;
        playerRB = PlayerMovement.Instance.playerRB;
    }

    private void FixedUpdate()
    {
        jumpInput = InputManager.Instance.jumpInput == 1;

        chargingSlingshot = onSlingShot && jumpInput;

        if (chargingSlingshot)
        {
            lr.enabled = true;

            if (slingShotBuffer < maxSlingShotBuffer)
                slingShotBuffer += Time.deltaTime;
            else
                slingShotBuffer = maxSlingShotBuffer;

            startPos = transform.position;
            endPos = new(slingShotBuffer * 100, slingShotBuffer * 100);
            Vector2 _velocity = (endPos - startPos).normalized * slingshotForce;

            Vector2[] trajectory = Plot(playerRB, transform.position, _velocity, steps);

            lr.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];

            for (int i = 0; i < trajectory.Length; i++)
                positions[i] = trajectory[i];

            lr.SetPositions(positions);
        }
        else if (!jumpInput)
        {
            slingShotBuffer = 0f;
            lr.enabled = false;
            endPos = new(slingShotBuffer * 100, slingShotBuffer * 100);
            velocity = (endPos - startPos).normalized * slingshotForce;
        }
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
            startPos = transform.position;

        if (Input.GetMouseButton(0))
        {
            lr.enabled = true;

            Vector2 endDragPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _velocity = (endDragPos - startPos).normalized * power;

            Vector2[] trajectory = Plot(playerRB, (Vector2)transform.position, _velocity, steps, maxDistance);

            lr.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];

            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }

            lr.SetPositions(positions);
        }
        else
        {
            lr.enabled = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 endDragPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _velocity = (endDragPos - startPos) * power;

            playerRB.velocity = _velocity;
        }
        */
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
            // if (distance >= maxDistance)
            //   break;

            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;

            distance += moveStep.magnitude;
        }

        return results;
    }
}
