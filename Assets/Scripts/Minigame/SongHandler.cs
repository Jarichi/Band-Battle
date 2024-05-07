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
    private int previewBeats = 0;
    private int index;

    public GameObject note;

    //prepare set velocity
    private NoteController noteController;

    private TriggerLine triggerLine;


    public Sprite[] noteColours;

    private bool scoreCalculated = false;


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

    //spawns notes based on a passed array of rhythm and note direction.
    private void SpawnNotes()
    {
        //create vector and offset value
        int xOffset = 0;
        Vector3 spawnPos = Vector3.zero;
       
        //if statement compares the current array index and the length of the entire array, AND it compares the number in the rhythm array to the current song position in beats
        //preview beats offsets the spawning moment by a certain beat number, so that the notes are visible n beats before they are supposed to be spawned.
        if (index < rhythm.Length && rhythm[index] < songPosBeats + previewBeats)
        {
            /*
            //debug statements (no longer nessecary
            Debug.Log("spawn note with direction");
            Debug.Log(inputDirection);
            */

            //read input direction array and determine offset and colour of note.
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
                    //error handler
                    Debug.Log("Load a beatmap first before spawning notes.");
                    break;
            }

            //set velocity of NoteController script based on the preview beats number, since notes are spawned 5 units above trigger line.
            //noteController.velocity = someNumber;

            //set spawn position offset relative to the songHandler object, and instantiate note object.
            spawnPos = transform.position + new Vector3(xOffset, 0f, 0f);
            Instantiate(note, spawnPos, Quaternion.identity);
            

            //increment index
            index++;
            
            
        } else if (index >= rhythm.Length && !scoreCalculated) {
            scoreCalculated = true;
            Debug.Log("Score: "); Debug.Log(GetScore()); 
            return;
        }
        

    }

    
    protected void SetNoteSequence(float[] p_rhythm, NoteDirection[] p_inputDirection)
    {       
        rhythm = p_rhythm;
        inputDirection = p_inputDirection;
    }

    private int GetScore()
    {
        return triggerLine.Score;
    }
}
