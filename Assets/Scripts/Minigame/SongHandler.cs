using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using https://www.gamedeveloper.com/audio/coding-to-the-beat---under-the-hood-of-a-rhythm-game-in-unity as main instruction

//Handles the current playing condition of any audio file.
public class SongHandler : MonoBehaviour
{
    public float songBPM, secondsPerBeat, songPosSeconds, songPosBeats, dspTime;
    public AudioSource musicSource;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();

        secondsPerBeat = 60f / songBPM;
        dspTime = (float)AudioSettings.dspTime;
    }

    private void setBeatTime()
    {
        songPosSeconds = (float)(AudioSettings.dspTime - dspTime);
        songPosBeats = songPosSeconds/secondsPerBeat;
    }

    private void Update()
    {
        setBeatTime();

    }

}
