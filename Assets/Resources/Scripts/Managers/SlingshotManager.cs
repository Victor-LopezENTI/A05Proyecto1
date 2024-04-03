using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotManager : MonoBehaviour
{

    private Rigidbody2D playerRB;
    private Projection projection;

    Camera cam;

    [SerializeField] float pushForce = 4f;

    bool isDragging = false;

    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    float distance;


    // Start is called before the first frame update
    void Start()
    {
        DeactivateRb(playerRB);
        cam = Camera.main;
    }

    // Update is called once per frame 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            OnDragStart();
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            OnDragEnd();
        }

        if (isDragging)
        {
            OnDrag();
        }
    }

    void OnDragStart() {
        DeactivateRb(playerRB);
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);

        projection.Show();
    }

    void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;

        projection.UpdateDots(playerRB.position, force);
    }

    void OnDragEnd()
    {
        ActivateRb(playerRB);

        Push(playerRB, force);

        projection.Hide();
    }
    public void Push(Rigidbody2D rb, Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb(Rigidbody2D rb)
    {
        rb.isKinematic = false;
    }

    public void DeactivateRb(Rigidbody2D rb)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }
}
