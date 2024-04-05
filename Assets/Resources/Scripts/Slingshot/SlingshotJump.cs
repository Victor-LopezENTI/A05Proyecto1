using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotJump : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;
    private Rigidbody2D playerRB;

    [SerializeField] private float power = 5f;
    [SerializeField] private int steps;
    [SerializeField] private float maxDistance;
    private Vector2 startDragPos;
    private Camera cam;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        playerRB = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            startDragPos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            lr.enabled = true;

            Vector2 endDragPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _velocity = (endDragPos - startDragPos).normalized * power;

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
            Vector2 _velocity = (endDragPos - startDragPos) * power;

            playerRB.velocity = _velocity;
        }
    }

    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps, float maxDistance)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        float distance = 0f;

        for (int i = 0; i < steps; i++)
        {
            if (distance >= maxDistance)
                break;

            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;

            distance += moveStep.magnitude;
        }

        return results;
    }

}
