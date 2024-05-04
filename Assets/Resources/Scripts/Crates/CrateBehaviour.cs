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
}