using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

//using https://www.gamedeveloper.com/audio/coding-to-the-beat---under-the-hood-of-a-rhythm-game-in-unity as main instruction

//Handles the current playing condition of any audio file.
public class SongHandler : MonoBehaviour
{

    public enum NoteDirection
    {
        Up, Down, Left, Right
    }

    /*private*/
    private float secondsPerBeat;
    private float songPosSeconds;
    private float songPosBeats;
    private float songStartTime;
    private AudioSource musicSource;

    
    public float bpm;

    private float[] rhythm;
    private NoteDirection[] inputDirection;
    private int previewBeats;
    private int index;


    public void InitRhythmGame()
    {
        //get amount of seconds in beat at a designated bpm
        Debug.Log(bpm); 

        secondsPerBeat = 60f / bpm;
        Debug.Log("calculate seconds per beat");
        //get time when the somg starts
        songStartTime = (float)AudioSettings.dspTime;
        

        //play song only when the script is loaded
        musicSource = GetComponent<AudioSource>();
        musicSource.Play();
    }

    private void SetSongPosition()
    {
        //subtract current time from starting time
        songPosSeconds = (float)(AudioSettings.dspTime - songStartTime);
        //convert time to beats
        songPosBeats = songPosSeconds/secondsPerBeat;
    }

    //start the game
    public void StartRhytmgame()
    {
        SetSongPosition();
        SpawnNotes();


    }

    //pass song position for creation of custom songs and rhythms
    private void SpawnNotes()
    {
       
        if (index < rhythm.Length && rhythm[index] < songPosBeats + previewBeats)
        {
            //Instantiate( /* Music Note Prefab */ );

            //initialize the fields of the music note
            Debug.Log("spawn note with direction");
            Debug.Log(inputDirection);
            index++;
        }

    }

    protected void SetNoteSequence(float[] p_rhythm, NoteDirection[] p_inputDirection)
    {
        
        rhythm = p_rhythm;
        inputDirection = p_inputDirection;
    }


}
