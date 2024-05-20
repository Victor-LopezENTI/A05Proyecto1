using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSound, sfxSound;
    [SerializeField] AudioSource MainMusic;
    [SerializeField] AudioSource SFXMusic;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    void Start()
    {
        PlayMusic("Initial");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x=> x.nameClip == name);

        if(s != null)
        {
            MainMusic.clip = s.clip;
            MainMusic.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSound, x => x.nameClip == name);

        if (s != null)
            SFXMusic.PlayOneShot(s.clip);        
    }

    public void MusicVolume(float volume)
    {
        MainMusic.volume = volume * 0.02f;
        PlayerPrefs.SetFloat("Music Volume", MainMusic.volume);
    }

    public void SFXVolume(float volume)
    {
        SFXMusic.volume = volume * 0.5f;
        PlayerPrefs.SetFloat("SFX Volume", SFXMusic.volume);
    }

    public void StopMusic()
    {
        MainMusic.Stop();
    }

    public void StopSFX(string name)
    {
        SFXMusic.Stop();
    }
}
