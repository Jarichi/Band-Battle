using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundRandom : MonoBehaviour
{
    public AudioClip[] sfx;
    public AudioSource sfxSource;
    public float pitchModifier = 0.2f;



    private void Start()
    {
        sfxSource = GetComponent<AudioSource>();

    }

    

    public void PlaySound()
    {
        sfxSource.clip = sfx[Random.Range(0, sfx.Length)];
        sfxSource.pitch = Random.Range(1-pitchModifier, 1+pitchModifier);
        sfxSource.Play();
    }
}
