using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RhythmTest : MonoBehaviour
{
    private Rhythm rhythm;
    [SerializeField] Song song;

    private void Start()
    {
        rhythm = GetComponent<Rhythm>();
        song.DeserializeFile();
        rhythm.noteSpawnEvent.AddListener((dir, inst) => print(dir + ", instrument = " + inst));
        rhythm.StartRhythm(song, GetComponent<AudioManager>(), Rhythm.ConsistentTimeEventTriggerRate.QuarterNote);
    }

}
