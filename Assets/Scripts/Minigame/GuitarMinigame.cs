using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarMinigame : Minigame
{
     //hardcoded song
    private float[] song1rhythm = { 1, 2, 3, 4, 4.5f, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 16};
    private NoteDirection[] song1direction = 
    { 
            NoteDirection.Up, 
            NoteDirection.Down, 
            NoteDirection.Left,
            NoteDirection.Right, 
            NoteDirection.Up, 
            NoteDirection.Down, 
            NoteDirection.Right, 
            NoteDirection.Left,
            NoteDirection.Right, 
            NoteDirection.Up, 
            NoteDirection.Down, 
            NoteDirection.Right, 
            NoteDirection.Up, 
            NoteDirection.Right, 
            NoteDirection.Down, 
            NoteDirection.Up,
            NoteDirection.Left

    };
    protected override string GetCombatAnimationName()
    {
        return "ObtainGuitar";
    }

    private void Start()
    {
        SetNoteSequence(song1rhythm, song1direction);
        //start mingame
        InitRhythmGame();
    }


    private void Update()
    {
        RunRhythmGame();
    }
}
