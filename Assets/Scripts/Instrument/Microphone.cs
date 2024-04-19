using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;


public class Microphone : Instrument
{
    public GameObject micEmpty;

    protected override void OnPlaying()
    {
        SpawnEmptyStand();
    }

    private void SpawnEmptyStand()
    {
        micEmpty.transform.position = transform.position;
        GameObject.Instantiate(micEmpty);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
