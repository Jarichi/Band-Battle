using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Android;




//using https://www.gamedeveloper.com/audio/coding-to-the-beat---under-the-hood-of-a-rhythm-game-in-unity as main instruction

//Responsible for handling music playback and tempo syncing
public class SongHandler : MonoBehaviour
{

    public enum NoteDirection
    {
        Up, Down, Left, Right
    }
    //used for handling tempo and song position
    private float secondsPerBeat;
    protected float songPosSeconds;
    protected float songPosBeats;
    private float songStartTime;
    private AudioSource musicSource;

    
    public float bpm;

    //used for note spawning
    private float[] rhythm;
    private NoteDirection[] inputDirection;
    private int previewBeats;
    private int index;
    public GameObject note;

    private TriggerLine triggerLine;


    public Sprite[] noteColours;
    



    //initialises rhythm game on call
    protected void InitRhythmGame()
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

    //updates current position and beat timer of the song
    private void SetSongPosition()
    {
        //subtract current time from starting time
        songPosSeconds = (float)(AudioSettings.dspTime - songStartTime);
        //convert time to beats
        songPosBeats = songPosSeconds/secondsPerBeat;
    }

    //when called, refreshes the game status at this moment
    protected void RunRhythmGame()
    {
        SetSongPosition();
        SpawnNotes();

    }

    //spawns notes based on a passed array of rhythm and note direction
    private void SpawnNotes()
    {
        //create vector and offset value
        int xOffset = 0;
        Vector3 spawnPos = Vector3.zero;
       
        if (index < rhythm.Length && rhythm[index] < songPosBeats + previewBeats)
        {

            //initialize the fields of the music note
            Debug.Log("spawn note with direction");
            Debug.Log(inputDirection);
            

            switch (inputDirection[index])
            {
                case NoteDirection.Left:
                    note.gameObject.GetComponent<SpriteRenderer>().sprite = noteColours[0];
                    //red note
                    xOffset = -2;
                    
                    break;
                case NoteDirection.Right:
                    //blue note
                    note.gameObject.GetComponent<SpriteRenderer>().sprite = noteColours[1];
                    xOffset = -1;
                    break;
                case NoteDirection.Up:
                    //yellow note
                    note.gameObject.GetComponent<SpriteRenderer>().sprite = noteColours[2];
                    xOffset = 0;
                    break;
                case NoteDirection.Down:
                    //green note
                    note.gameObject.GetComponent<SpriteRenderer>().sprite = noteColours[3];
                    xOffset = 1;
                    break;

                default:
                    Debug.Log("Load a beatmap first before spawning notes.");
                    break;

            }
            spawnPos = transform.position + new Vector3(xOffset, 0f, 0f);
            Instantiate(note, spawnPos, Quaternion.identity);

            index++;
            //(hopefully) when no more content in the rhythm game, get the score and terminate.
        } else if (index >= rhythm.Length) {
            Debug.Log("Score: "); Debug.Log(GetScore()); 
            return;
        }
        

    }

    //passes the array of rhythm and note direction
    protected void SetNoteSequence(float[] p_rhythm, NoteDirection[] p_inputDirection)
    {       
        rhythm = p_rhythm;
        inputDirection = p_inputDirection;
    }

    protected int GetScore()
    {
        return triggerLine.Score;
    }
}
