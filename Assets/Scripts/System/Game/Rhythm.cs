using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Rhythm : MonoBehaviour
{
    public enum NoteDirection
    {
        Up, Down, Left, Right
    }
    public enum ConsistentTimeEventTriggerRate
    {
        WholeNote, HalfNote, QuarterNote, EightNote, SixteenthNote
    }

    public bool Active { get; private set; }
    public double BPM { get; private set; }

    // songPos
    private double secondsPerBeat;
    [SerializeField]
    private double songPosInSeconds;
    [SerializeField]
    private double songPosInBeats;

    private double songStartTime;

    private const double MINUTE = 60f;

    // beatmap
    private NoteInfo[] beatmapNotes;
    private int index;
    public double previewBeats;

    //beattracker 
    public float moduloIndex;
    private double lastRemainder;
    private double remainder;

    //event
    public UnityEvent rhythmStartEvent;
    public UnityEvent<NoteDirection, string> noteSpawnEvent;
    public UnityEvent<double> consistentTimeEvent;
    public UnityEvent rhythmEndEvent;

    public void StartRhythm(Song song, AudioManager audio, ConsistentTimeEventTriggerRate eventTriggerRate)
    {
        List<List<NoteInfo>> infos = new();
        song.beatmap.Channels.ForEach(channel =>
        {
            infos.Add(channel.Positions.ConvertAll(position => new NoteInfo(position.Beat, (NoteDirection)position.Direction, channel.InstrumentID)));
        });
        beatmapNotes = infos.SelectMany(n => n).OrderBy(n => n.beat).ToArray();

        switch (eventTriggerRate)
        {
            case ConsistentTimeEventTriggerRate.WholeNote:
                moduloIndex = 4;
                break;
            case ConsistentTimeEventTriggerRate.HalfNote:
                moduloIndex = 2;
                break;
            case ConsistentTimeEventTriggerRate.QuarterNote:
                moduloIndex = 1;
                break;
            case ConsistentTimeEventTriggerRate.EightNote:
                moduloIndex = 0.5f;
                break;
            case ConsistentTimeEventTriggerRate.SixteenthNote:
                moduloIndex = 0.25f;
                break;
        }

        BPM = song.beatmap.BPM;
        secondsPerBeat = MINUTE / BPM;
        songStartTime = Time.time;
        audio.Play();
        StartCoroutine(End(audio.GetLength()));
        Active = true;
        rhythmStartEvent?.Invoke();
    }
    private IEnumerator End(double songLength)
    {
        yield return new WaitForSeconds((float)songLength);
        Active = false;
        rhythmEndEvent?.Invoke();
    }

    private void Update()
    {
        if (Active)
        {
            UpdateSongPosition();
            TrackBeatmapEvents();
            TrackBeats();
        }
    }


    private void UpdateSongPosition()
    {
        songPosInSeconds = (Time.time - songStartTime);
        songPosInBeats = songPosInSeconds / secondsPerBeat;
    }

    private void TrackBeatmapEvents()
    {
        if (index < beatmapNotes.Length && beatmapNotes[index].beat < songPosInBeats + previewBeats)
        {
            noteSpawnEvent?.Invoke(beatmapNotes[index].direction, beatmapNotes[index].instrumentID);
            index++;
        }
    }

    private void TrackBeats()
    {
        lastRemainder = remainder;
        remainder = (songPosInBeats + previewBeats) % moduloIndex;

        if (lastRemainder > remainder)
        {
            consistentTimeEvent?.Invoke(songPosInBeats);
        }
    }

    private class NoteInfo
    {
        internal double beat;
        internal NoteDirection direction;
        internal string instrumentID;

        internal NoteInfo(double beat, NoteDirection direction, string instrumentID)
        {
            this.beat = beat;
            this.direction = direction;
            this.instrumentID = instrumentID;
        }
    }
}
