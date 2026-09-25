using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Godot.Collections;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Text.Json;

[Tool]
public partial class MIDI2Beatmap : EditorPlugin
{
	private string outputPath = "res://addons/midi2beatmap/out/";
	private string midiFilePath = "res://audio/midi/";
	private InspectorMIDI2Beatmap inspector;

	public override void _EnterTree()
	{
		// Register inspector plugin to show Convert UI when the script is selected
		inspector = new InspectorMIDI2Beatmap(this);
		AddInspectorPlugin(inspector);
	}

	public override void _ExitTree()
	{
		// Remove the inspector plugin when this plugin is disabled/unloaded.
		if (inspector != null)
		{
			RemoveInspectorPlugin(inspector);
			inspector = null;
		}
	}

	public void Convert(string midiPath, string outPath)
	{
		var fullMidi = ProjectSettings.GlobalizePath(midiPath);
		var fullOut = ProjectSettings.GlobalizePath(outPath);

		if (!System.IO.File.Exists(fullMidi))
		{
			GD.PushError($"MIDI file was not found: {midiPath} ({fullMidi})");
			return;
		}

		var midi = MidiFile.Read(fullMidi);
		var map = midi.GetTempoMap();

		var notes = midi.GetNotes();

		var tracks = new List<BeatmapTrack>();

		// Try to read a SetTempoEvent from the file to determine BPM. Fall back to 120 if none found.
		double tempo = 120.0;
		var tempoEvent = midi.GetTrackChunks()
			.SelectMany(tc => tc.Events)
			.OfType<SetTempoEvent>()
			.FirstOrDefault();
		if (tempoEvent != null && tempoEvent.MicrosecondsPerQuarterNote > 0)
		{
			tempo = 60000000.0 / tempoEvent.MicrosecondsPerQuarterNote;
		}

		var channels = midi.GetChannels().ToList();
		for (int i = 0; i < channels.Count; i++)
		{
			var positions = new List<BeatmapNote>();
			foreach (var note in notes)
			{
				if (note.Channel == i)
				{
					var time = note.TimeAs<BarBeatFractionTimeSpan>(map);
					var beatTotal = (time.Bars * 4) + time.Beats;
					positions.Add(new BeatmapNote { Column = note.NoteNumber % 4, Beat = beatTotal });
				}
			}
			tracks.Add(new BeatmapTrack { Notes = positions });
		}

		var beatmap = new Beatmap { BPM = tempo, Tracks = tracks };

		var json = JsonSerializer.Serialize(beatmap, new JsonSerializerOptions
		{
			WriteIndented = true,
			IncludeFields = true
		});

		Directory.CreateDirectory(fullOut);
		var path = System.IO.Path.Combine(fullOut, "out.bbm");
		System.IO.File.WriteAllText(path, json);
		GD.Print($"Successfully created beatmap at {path}");
	}
}
