using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] PlayerInput playerInput;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Collider2D playerCollider;

    [SerializeField] GameObject groundChk;

    [SerializeField] LayerMask groundLayer;

    float distanceFromGround, gravityMultiplier;

    Vector2 auxVelocity, boxCastSize;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gravityMultiplier = 1f;
        distanceFromGround = 0.025f;
        boxCastSize = new Vector2(0.14f, 0.1f);
    }
    private void FixedUpdate()
    {
        
    }

    public bool isGrounded()
    {
        return Physics2D.BoxCast(new Vector2(groundChk.transform.position.x, groundChk.transform.position.y + 0.05f), boxCastSize, 0f, Vector2.down, distanceFromGround, groundLayer);
    }
}
