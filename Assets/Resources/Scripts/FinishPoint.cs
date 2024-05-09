using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    [SerializeField]
    private bool goNextLevel;
    [SerializeField]
    private string LevelName;
    [SerializeField]
    private bool zone1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(goNextLevel)
                SceneController.instance.NextLevel();
            else
                SceneController.instance.LoadScene(LevelName);

            AudioManager.Instance.PlaySFX("Finish");

            if (zone1)
            {
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlayMusic("Zone1");
            }
        }
    }
}