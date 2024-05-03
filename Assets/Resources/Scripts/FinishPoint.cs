using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [SerializeField]
    private bool goNextLevel;
    [SerializeField]
    private string LevelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(goNextLevel)
                SceneController.instance.NextLevel();
            else
                SceneController.instance.LoadScene(LevelName);
        }
    }
}