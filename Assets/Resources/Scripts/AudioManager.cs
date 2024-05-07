using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MainMenuMusic;
    [SerializeField] AudioSource SFXMenuMusic;

    private bool menu;

    void Start()
    {
        MainMenuMusic.Play();
    }

    private void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "MainMenuScene")
            Destroy(gameObject);

        else
            DontDestroyOnLoad(gameObject);

    }

}
