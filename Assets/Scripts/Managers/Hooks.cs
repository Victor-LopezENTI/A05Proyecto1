using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hooks : MonoBehaviour
{
    private float hooks = 0;

    [SerializeField] private Text Pickuptext;
    [SerializeField] private SpawnerHooks spHook;
    [SerializeField] private GameObject obj;

    private bool canPickUp;
    void Start()
    {
        canPickUp = false;
        //Pickuptext.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPickUp == true)
        {
            spHook.SetHooks();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            //Pickuptext.gameObject.SetActive(true);
            canPickUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            //Pickuptext.gameObject.SetActive(false);
            canPickUp = false;
        }
    }
}
