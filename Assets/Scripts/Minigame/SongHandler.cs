using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
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
    [SerializeField]
    protected float songPosInSeconds;
    [SerializeField]
    protected float songPosInBeats;

    private float songStartTime;
    private AudioSource musicSource;

    public AudioClip spawnNoteSFX;

    
    public float bpm;
    private const float FRAMERATE = 60f;

    //used for note spawning
    private float[] rhythm;
    private NoteDirection[] inputDirection;
    public int previewBeats;
    private int index;

    public GameObject note;

    //prepare set velocity
    private NoteController noteController;

    private readonly TriggerLine triggerLine;


    public Sprite[] noteColours;

    private bool scoreCalculated = false;



    //initialises rhythm game on call
    protected void InitRhythmGame()
    {
        secondsPerBeat = FRAMERATE / bpm;

        
        //play song only when the script is loaded
        musicSource = GetComponent<AudioSource>();

        print(Time.deltaTime);

        musicSource.Play();
        //songStartTime = (float)AudioSettings.dspTime; //OUT OF SYNC
        songStartTime = Time.time;
    }

    //updates current position and beat timer of the song
    private void SetSongPosition()
    {
        //songPosInSeconds = (float)(AudioSettings.dspTime - songStartTime); //OUT OF SYNC
        songPosInSeconds = (Time.time - songStartTime);

        songPosInBeats = songPosInSeconds/secondsPerBeat;
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
        float xOffset = 0;
        Vector3 spawnPos = Vector3.zero;
       
        //if statement compares the current array index and the length of the entire array, AND it compares the number in the rhythm array to the current song position in beats
        //preview beats offsets the spawning moment by a certain beat number, so that the notes are visible n beats before they are supposed to be spawned.
        if (index < rhythm.Length && rhythm[index] < songPosInBeats + previewBeats)
        {
            //musicSource.clip = spawnNoteSFX;
            //musicSource.Play();
            //read input direction array and determine offset and colour of note.
            switch (inputDirection[index])
            {
                case NoteDirection.Left:
                    note.gameObject.GetComponent<SpriteRenderer>().sprite = noteColours[0];
                    //red note
                    xOffset = -1.5f;
                    
                    break;
                case NoteDirection.Right:
                    //blue note
                    note.gameObject.GetComponent<SpriteRenderer>().sprite = noteColours[1];
                    xOffset = -0.5f;
                    break;
                case NoteDirection.Up:
                    //yellow note
                    note.gameObject.GetComponent<SpriteRenderer>().sprite = noteColours[2];
                    xOffset = 0.5f;
                    break;
                case NoteDirection.Down:
                    //green note
                    note.gameObject.GetComponent<SpriteRenderer>().sprite = noteColours[3];
                    xOffset = 1.5f;
                    break;

                default:
                    //error handler
                    Debug.Log("Load a beatmap first before spawning notes.");
                    break;
            }

            //set spawn position offset relative to the songHandler object, and instantiate note object.
            spawnPos = transform.position + new Vector3(xOffset, 0f, 0f);
            Instantiate(note, spawnPos, Quaternion.identity, this.transform);

            index++;

        } 
        else if (index >= rhythm.Length && !scoreCalculated) 
        {

            scoreCalculated = true;
            Debug.Log("Score: ");
            Debug.Log(GetScore());
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
