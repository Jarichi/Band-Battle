using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class MIDI2Beatmap : EditorWindow
{
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

        for (int i = 0; i < 4; i++)
        {
            foreach (var note in notes)
            {
                if (note.Channel == i)
                {
                    var time = note.TimeAs<BarBeatFractionTimeSpan>(map);
                    Debug.Log("CHANNEL : " + note.Channel + "| NOTE : " + note.NoteName + "| BAR : " + time.Bars + "| BEAT : " + time.Beats);
                }
            }
        }
    }
}
