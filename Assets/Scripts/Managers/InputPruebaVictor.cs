using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPruebaVictor : MonoBehaviour
{
    private float horizontal;
    public float speed;
    public float jump;

    [SerializeField] private Rigidbody2D rg2;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;

    public void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            rg2.velocity = new Vector2(rg2.velocity.x, jump);
        }

        if (Input.GetButtonDown("Jump") && rg2.velocity.y>0)
        {
            rg2.velocity = new Vector2(rg2.velocity.x, rg2.velocity.y * 0.5f);
        }
    }
    private void FixedUpdate()
    {
        rg2.velocity = new Vector2(horizontal * speed, rg2.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);
    }

    public float horizontalMovement(InputAction.CallbackContext context)
    {
        return context.ReadValue<float>();
    }
}
