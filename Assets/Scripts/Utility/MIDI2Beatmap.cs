using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Beatmap;

public class MIDI2Beatmap : EditorWindow
{
    private string outputPath = "Assets/Scripts/Minigame/Beatmap/";
    private string midiFilePath = "Assets/Audio/MIDI/";

    [MenuItem("Tools/MIDI to Beatmap converter")]
    public static void ShowWindow()
    {
        GetWindow<MIDI2Beatmap>("Tools/MIDI to Beatmap converter");
    }

    void OnGUI()
    {
        GUILayout.Label("MIDI file path", EditorStyles.boldLabel);
        midiFilePath = EditorGUILayout.TextField(midiFilePath);
        GUILayout.Label("Output file path", EditorStyles.boldLabel);
        outputPath = EditorGUILayout.TextField(outputPath);
        if (GUILayout.Button("Convert"))
        {
            Convert();
        }
    }

    void Convert()
    {
        File.SetAttributes(midiFilePath, FileAttributes.Normal);
        var midi = MidiFile.Read(midiFilePath);
        var map = midi.GetTempoMap();

        var notes = midi.GetNotes();

        var channels = new List<Channel>();
        double tempo = 0f;
        for (int i = 0; i < midi.GetChannels().Count(); i++)
        {
            List<NotePosition> positions = new List<NotePosition>();
            foreach (var note in notes)
            {
                if (note.Channel == i)
                {
                    var time = note.TimeAs<BarBeatFractionTimeSpan>(map);
                    tempo = midi.GetTempoMap().GetTempoAtTime(time).BeatsPerMinute;
                    var beatTotal = (time.Bars * 4) + time.Beats;
                    positions.Add(new NotePosition(note.NoteNumber % 4, beatTotal));
                    //Debug.Log("CHANNEL : " + note.Channel + "| NOTE : " + note.NoteName + "| BAR : " + time.Bars + "| BEAT : " + time.Beats);
                }
            }
            channels.Add(new Channel(i, positions));
        }
        var beatmap = new Beatmap(tempo, channels);
        Debug.Log(beatmap.Channels.Count);
        string jsonString = JsonUtility.ToJson(beatmap, true);
        var path = outputPath + "out.bbm";

        File.WriteAllText(path, jsonString);
        Debug.Log(jsonString);
    }
}

[Serializable]
public class Beatmap
{
    public double BPM;
    [SerializeField]
    public List<Channel> Channels;

    public Beatmap(double bpm, List<Channel> channels) { 
        BPM = bpm;  Channels = channels; 
    }
    
}

[Serializable]
public class Channel
{
    public int Number;
    [SerializeField]
    public List<NotePosition> Positions;

    public Channel(int number, List<NotePosition> positions)
    {
        this.Number = number;
        this.Positions = positions;
    }
}

[Serializable]
public class NotePosition
{
    public int Direction;
    public double Beat;

    public NotePosition(int direction, double beat)
    {
        this.Direction = direction;
        this.Beat = beat;
    }
}