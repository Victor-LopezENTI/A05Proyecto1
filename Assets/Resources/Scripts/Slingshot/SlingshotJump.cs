using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotJump : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;

    private Rigidbody2D playerRB;

    [SerializeField] private float slingshotForce = 8f;
    [SerializeField] private int steps = 500;

    PlayerStateMachine playerStateMachine;

    [SerializeField] public bool onSlingShot { get; private set; } = true;
    [SerializeField] private bool chargingSlingshot = false;

    private Vector2 dragStartPos;

    // Final velocity
    public Vector2 velocity { get; private set; }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        playerStateMachine = PlayerStateMachine.Instance;
        playerRB = PlayerMovement.Instance.playerRB;
    }

    void Update()
    {
        if (onSlingShot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                chargingSlingshot = true;
                lr.enabled = true;
                dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                velocity = (dragEndPos - dragStartPos) * slingshotForce;
                Vector2[] trajectory = Plot(playerRB, playerRB.position, velocity, steps);
                lr.positionCount = trajectory.Length;

                Vector3[] positions = new Vector3[trajectory.Length];
                for (int i = 0; i < trajectory.Length; i++)
                    positions[i] = trajectory[i];

                lr.SetPositions(positions);
            }

            if (Input.GetMouseButtonUp(0))
            {
                chargingSlingshot = false;
                lr.enabled = false;
                Vector2 dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                velocity = (dragEndPos - dragStartPos) * slingshotForce;
            }
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
/*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slingshot")
            onSlingShot = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slingshot")
            onSlingShot = false;
    }
*/
}