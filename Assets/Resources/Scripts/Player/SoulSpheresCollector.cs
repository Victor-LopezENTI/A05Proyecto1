using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpheresCollector : MonoBehaviour
{
    private int soulSphereCounter = 0;
   

    void OnTriggerEnter2D(Collider2D soulSphere)
    {
        if (soulSphere.gameObject.CompareTag("Soul")) 
        {
            Debug.Log("Soul Sphere Collected. Counter: " + ++soulSphereCounter);
            Destroy(soulSphere.gameObject); 
        }
    }

}
