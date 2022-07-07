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
    public AudioClip confetti;
    public AudioClip vomiting;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip star;
    public AudioClip stepComplete;
    public AudioClip newItemUnlock;
    public AudioClip buttonClick;
    public List<AudioClip> eatGoodReactions;

    [Header("UI RELATED")] public AudioClip moneyEarned;
    public AudioClip moneyAdding;
    public List<AudioClip> gridSpawnPops;

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

    public void PlayEatingGoodReactions()
    {
        sfxAudioSource.PlayOneShot(eatGoodReactions[Random.Range(0, eatGoodReactions.Count)]);
    }

    private bool _canPlayPops = true;

    public IEnumerator PlayGridSpawnPops()
    {
        if (_canPlayPops)
        {
            sfxAudioSource.PlayOneShot(gridSpawnPops[Random.Range(0, gridSpawnPops.Count)]);
            yield return new WaitForSeconds(.35f);
        }

        _canPlayPops = false;
        yield return new WaitForSeconds(1.5f);
        _canPlayPops = true;
    }
}