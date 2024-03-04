using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class SpawnerHooks : MonoBehaviour
{
    private float hooks = 5;

    [SerializeField] private Transform objSpawn;
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject objContainer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && hooks >0)
        {
            GameObject HookClone = Instantiate(obj,objSpawn.position,objSpawn.rotation);
            HookClone.transform.parent = objContainer.transform;
            hooks--;
        }
    }

    public void SetHooks()
    {
        hooks++;
        var coins = new List<GameObject>();
        foreach (Transform child in objContainer.transform) coins.Add(child.gameObject);
        coins.ForEach(child => Destroy(child));
    }
}
