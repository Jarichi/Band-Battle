using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;


public class Microphone : Instrument
{
    private bool spawned = false;


    public GameObject micEmpty;

    protected override void OnPlaying()
    {
        while (!spawned)
        {
            GameObject.Instantiate(micEmpty);
            spawned = true;
        }
    }
}
