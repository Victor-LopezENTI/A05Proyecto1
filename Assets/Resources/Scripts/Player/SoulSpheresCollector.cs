using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpheresCollector : MonoBehaviour
{
    private int soulSphereCounter = 0;
   

    void OnTriggerEnter(Collider soulSphere)
    {
        if (soulSphere.CompareTag("Soul")) 
        {
            soulSphereCounter++;
            Debug.Log("Soul Sphere Collected. Counter: " + soulSphereCounter);
            Destroy(soulSphere.gameObject); 
        }
    }

}
