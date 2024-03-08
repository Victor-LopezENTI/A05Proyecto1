using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class SpawnerHooks : MonoBehaviour
{
    private int hooks = 5;

    [SerializeField] private Transform objSpawn;
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject objContainer;
    [SerializeField] private Collider2D col;
    private GameObject HookClone;
    //[SerializeField] private List<GameObject> HooksInContact = new List<GameObject>();


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && hooks >0)
        {
            HookClone = Instantiate(obj,objSpawn.position,objSpawn.rotation);
            HookClone.transform.parent = objContainer.transform;
            hooks--;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetHooks();
        }
    }

    public void SetHooks()
    {
        //int hookCount = HooksInContact.Count;
        //for (int i = 0; i < hookCount; i++)
        //{
        //    hooks++;
        //    //GameObject tempHook = HooksInContact[0];
        //    //HooksInContact.Remove(tempHook);
        //    Destroy(HooksInContact[0]);
        //}
        //foreach (GameObject hook in HooksInContact)
        //{
        //    hooks++;
        //    HooksInContact.Remove(hook);
        //    Destroy(hook);
        //}

        col.enabled = true;
        StartCoroutine(disableAfterDelay(0.01f));

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            //Pickuptext.gameObject.SetActive(true);
            //canPickUp = true;
            //HooksInContact.Add(collision.gameObject);
            hooks++;
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            //Pickuptext.gameObject.SetActive(false);
            //canPickUp = false;
            //HooksInContact.Remove(collision.gameObject);
        }
    }
    private IEnumerator disableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        col.enabled = false;
    }
}
