using System;
using UnityEngine;
using UnityEngine.Events;

//class will be reused as note spawnpoint, and will only act on function calls or events

//using https://www.gamedeveloper.com/audio/coding-to-the-beat---under-the-hood-of-a-rhythm-game-in-unity as main instruction

//Responsible for handling music playback and tempo syncing
public abstract class SongHandler : MonoBehaviour
{
    
    public enum NoteDirection
    {
        Up, Down, Left, Right
    }

    //used for handling tempo and song position
    //TODO: doublecjecg everythnBDJFg
    private double secondsPerBeat;
    [SerializeField]
    private double songPosInSeconds;
    [SerializeField]
    private double songPosInBeats;

    private double songStartTime;

    protected double bpm;
    private const double MINUTE = 60f;
    private const int BAR = 4;
    private double[] rhythm;
    private NoteDirection[] inputDirection;
    public double previewBeats;
    private int index;
    [SerializeField]
    private GameObject note;
    [SerializeField]
    //private GameObject triggerLine;
    public Sprite[] noteColours;
    private bool scoreCalculated = false;
    private bool initCountdown = false;

    public static UnityEvent globalBeatEvent;

    public enum PulseDivisions
    {
        WholeNote,
        HalfNote,
        QuarterNote,
        EightNote,
        SixteenthNote
    }
    public float moduloIndex;
    [SerializeField] public PulseDivisions pulseDivisions;
    private double lastRemainder;
    private double remainder;


    //initialises rhythm game on call
    protected void InitRhythmGame()
    {
        secondsPerBeat = MINUTE / bpm;

        //songStartTime = (double)AudioSettings.dspTime; //OUT OF SYNC
        songStartTime = Time.time;

        SetPulseDividerSpawnrate(pulseDivisions);
    }

    //updates current position and beat timer of the song
    private void SetSongPosition()
    {
        songPosInSeconds = (Time.time - songStartTime);

        songPosInBeats = songPosInSeconds / secondsPerBeat;
    }

    //when called, refreshes the game status at this moment
    protected void RunRhythmGame()
    {
        SetSongPosition();
        SpawnNotes();
        SpawnPulseDivider();


    }

    //spawns notes based on a passed array of rhythm and note direction.
    private void SpawnNotes()
    {
        //create vector and offset value
        float xOffset = 0;
        Vector3 spawnPos = Vector3.zero;

        if (!initCountdown)
        {
            //offset all entries in the rhythm array by one bar.
            for (int i = 0; i <= rhythm.Length - 1; i++)
            {
                rhythm[i] += BAR;
                //print(rhythm[i]);

            }
            //print("finish");

            initCountdown = true;

        }


        //if statement compares the current array index and the length of the entire array, AND it compares the index in the rhythm array to the current song position in beats
        //preview beats offsets the spawning moment by a certain beat index, so that the notes are visible n beats before they are supposed to be spawned.
        if (index < rhythm.Length && rhythm[index] < songPosInBeats + previewBeats)
        {

            //read Input direction array and determine offset and colour of note.
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
            note.gameObject.GetComponent<NoteController>().SetDirection(inputDirection[index]);
            Instantiate(note, spawnPos, Quaternion.identity, this.transform);

            

            index++;

        }
        else if (index >= rhythm.Length && !scoreCalculated)
        {
            if (GameObject.FindGameObjectWithTag("minigame_Note") == null)
            {
                scoreCalculated = true;
                Game.Instance.End();
                return;
            }

        }



    }


    protected void SetNoteSequence(double[] p_rhythm, NoteDirection[] p_inputDirection)
    {
        rhythm = p_rhythm;
        inputDirection = p_inputDirection;
    }

    public double GetBPM()
    {
        return bpm;
    }

    private void SetPulseDividerSpawnrate(PulseDivisions division)
    {
        switch (division)
        {
            case PulseDivisions.WholeNote:

                moduloIndex = 4;
                break;
            case PulseDivisions.HalfNote:
                moduloIndex = 2;
                break;
            case PulseDivisions.QuarterNote:
                moduloIndex = 1;
                break;
            case PulseDivisions.EightNote:
                moduloIndex = 0.5f;
                break;
            case PulseDivisions.SixteenthNote:
                moduloIndex = 0.25f;
                break;
        }
        print("modulo index = " + moduloIndex);

        return;
    }

    private void SpawnPulseDivider()
    {
        
        lastRemainder = remainder;
        remainder = (songPosInBeats + previewBeats) % moduloIndex;

        if (lastRemainder > remainder)
        {
            //print("triggered");
            var pulsemarker = Resources.Load<GameObject>("Prefabs/Minigame/Pulse Marker");
            print(pulsemarker);
            Instantiate(pulsemarker, this.transform);

            globalBeatEvent.Invoke(); //launches global event
        }
    }
}
