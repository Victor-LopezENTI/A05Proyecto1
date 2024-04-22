using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hooks : MonoBehaviour
{
    //private float hooks = 0;

    [SerializeField] private Text Pickuptext;
    //public SpawnerHooks spHook;
    //[SerializeField] private GameObject obj;

    //private bool canPickUp;
    void Start()
    {
        //canPickUp = false;
        //Pickuptext.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Pickuptext.gameObject.SetActive(true);
            //canPickUp = true;
            //spHook.HooksInContact.Add(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Pickuptext.gameObject.SetActive(false);
            //canPickUp = false;
            //spHook.HooksInContact.Remove(this.gameObject);
        }
    }
}