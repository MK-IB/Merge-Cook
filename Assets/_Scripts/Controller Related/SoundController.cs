using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;

    public AudioSource sfxAudioSource;
    public AudioSource ambientAudioSource;
    public bool isBeachEnv;

    public AudioClip natureAmbience;
    public AudioClip beachAmbience;
    public AudioClip swipe;
    public AudioClip spawn;
    public AudioClip blast;
    public AudioClip explosionFinal;
    public AudioClip eating;
    public AudioClip wow;
    public AudioClip girlYummy;
    public AudioClip confetti;
    public AudioClip vomit;
    public AudioClip win;
    public AudioClip lose;
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        /*if (isBeachEnv) ambientAudioSource.clip = beachAmbience;
        else ambientAudioSource.clip = natureAmbience;
        ambientAudioSource.enabled = true;*/
    }

    public void PlayClip(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }
}
