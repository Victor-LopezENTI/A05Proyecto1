using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MainMenuMusic;
    [SerializeField] AudioSource SFXMenuMusic;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        MainMenuMusic.Play();
    }

}
