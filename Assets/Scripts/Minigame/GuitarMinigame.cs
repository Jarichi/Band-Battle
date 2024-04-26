using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarMinigame : Minigame
{
     //hardcoded song
    private float[] song1rhythm = { 1, 2, 3, 4, 4.5f, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15 };
    private char[] song1direction = { 'u', 'd', 'l', 'r', 'r', 'u', 'r', 'u', 'r', 'u', 'u', 'r', 'u', 'r', 'u', 'u', 'd'};
    protected override string GetCombatAnimationName()
    {
        return "ObtainGuitar";
    }

    private void Start()
    {
        SetNoteSequence(song1rhythm, song1direction);
        
    }

}
