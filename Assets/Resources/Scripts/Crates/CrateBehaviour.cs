using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrateBehaviour : MonoBehaviour
{
    private Rigidbody2D crateRB;

    private void Awake()
    {
        crateRB = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        crateRB.AddForce(Vector2.right * 10f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}