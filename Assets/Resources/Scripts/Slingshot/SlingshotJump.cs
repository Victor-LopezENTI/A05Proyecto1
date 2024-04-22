using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SlingshotJump : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;

    private Rigidbody2D playerRB;
    
    [SerializeField] private float slingshotForce = 5f;
    [SerializeField] private int steps = 500;
    

    PlayerStateMachine playerStateMachine;

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
        chargingSlingshot = onSlingShot;
    }

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

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
            startPos = transform.position;

        if (Input.GetMouseButton(0))
        {
            lr.enabled = true;

            Vector2 endDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _velocity = (endDragPos - startPos) * slingshotForce;

            Vector2[] trajectory = Plot(playerRB, (Vector2)transform.position, _velocity, steps, 30f);

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
            Vector2 endDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _velocity = (endDragPos - startPos) * slingshotForce;

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