using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node/next.svg")]
public partial class BeatmapPlayer : Node
{

	[Export]
	private SyncedMusicPlayer _clock;

	[Export]
	private double _spawnLeadBeats = 4.1;

	private double _currentBeat;
	private Beatmap _beatmap;
	private BeatmapNote[] _notes;
	private int _currentNoteIndex = 0;


	[Signal]
	public delegate void NoteEventHandler(int column, double spawnBeat, double hitBeat);

	[Signal]
	public delegate void BeatEventHandler(double beatPosition);


	public override void _Ready()
	{
		_beatmap = BeatmapLoader.LoadFromFile("flower_man.json");
		GD.Print($"Loaded beatmap with {_beatmap.Tracks.Count} tracks and BPM: {_beatmap.BPM}");
		_notes = [.. _beatmap.Tracks[0].Notes];
		_clock.Start((float)_beatmap.BPM);
		_clock.Beat += OnBeat;
	}

	public override void _ExitTree()
	{
		_clock.Beat -= OnBeat;
	}

	public void OnBeat(double beatPosition)
	{
		_currentBeat = beatPosition - _clock.BeatOffset;
		EmitSignal(SignalName.Beat, _currentBeat);

		while (_currentNoteIndex < _notes.Length)
		{
			var note = _notes[_currentNoteIndex];
			double spawnBeat = note.Beat - _spawnLeadBeats;

			if (spawnBeat > _currentBeat)
			{
				break;
			}

			EmitSignal(SignalName.Note, note.Column, spawnBeat, note.Beat);
			_currentNoteIndex++;
		}

	}
}
