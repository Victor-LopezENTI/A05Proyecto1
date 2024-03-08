using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public static PlayerMovement instance;

    // MANAGERS
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Collider2D playerCollider;

    // MOVEMENT VARIABLES
    [SerializeField] public float moveSpeed, jumpForce;
    [SerializeField] private bool onGround, jumping;
    private Vector2 playerVelocity;

    // GROUNDCHECK
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] float distanceFromGround;
    Vector2 boxCastSize;

    #endregion

    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;

        // Get Components
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        distanceFromGround = 0.185f;
        boxCastSize = new(0.14f, 0.1f);

        moveSpeed = 500f;
        jumpForce = 300f;
    }

    private void Update()
    {

        Debug.DrawRay(groundCheck.transform.position, new Vector2(0f, -distanceFromGround), Color.green);
    }

    private void FixedUpdate()
    {
        onGround = isGrounded();    // Check if the player is touching the ground

        jumping = inputManager.jumpInput == 1;

        // Jump
        if (jumping)
        {
            playerVelocity = new(inputManager.moveInput * moveSpeed * Time.deltaTime, inputManager.jumpInput * jumpForce * Time.deltaTime);
        }
        else
        {
            playerVelocity = new(inputManager.moveInput * moveSpeed * Time.deltaTime, playerRB.velocity.y);
        }

        playerRB.velocity = playerVelocity;

        // Jump interaction test
        if (inputManager.interacted == 1)
        {
            transform.DOJump(new(playerRB.position.x + 10f, playerRB.position.y), 2f, 1, 0.8f).SetEase(Ease.Linear);
        }
    }

    public bool isGrounded()
    {
        return Physics2D.BoxCast(groundCheck.transform.position, boxCastSize, 0f, Vector2.down, distanceFromGround, groundLayer);
    }
}