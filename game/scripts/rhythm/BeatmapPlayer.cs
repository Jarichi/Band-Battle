using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass, Icon("res://addons/at-icons/node/next.svg")]
public partial class BeatmapPlayer : Node
{

	[Export]
	private MusicClock _clock;

	[Export]
	private double _spawnLeadBeats = 4.1;

	[Export] private double _nearWindow = 0.2;
	[Export] private double _perfectWindow = 0.05;
	[Export] private double _despawnAfter = 0.2;

	private double _currentBeatWithOffset;
	private Beatmap _beatmap;
	private BeatmapNote[] _notes;
	private int _currentNoteIndex = 0;
	private int _nextMeasureBeat = 0;
	private readonly List<ActiveNote> _spawnedNotes = new();
	public IReadOnlyList<ActiveNote> ActiveNotes => _spawnedNotes.AsReadOnly();
	public double SpawnLeadBeats => _spawnLeadBeats;

	[Signal]
	public delegate void NoteEnterEventHandler(int index, int column, double spawnedOn, double hitBeat);

	[Signal]
	public delegate void NoteLeaveEventHandler(int index, int column);


	public override void _Ready()
	{
		_beatmap = BeatmapLoader.LoadFromFile("flower_man.json");
		GD.Print($"Loaded beatmap with {_beatmap.Tracks.Count} tracks and BPM: {_beatmap.BPM}");
		_notes = [.. _beatmap.Tracks[0].Notes];
		_clock.Start((float)_beatmap.BPM);
	}

	public override void _PhysicsProcess(double delta)
	{
		_currentBeatWithOffset = _clock.GetCurrentBeat();

		while (_nextMeasureBeat - _spawnLeadBeats <= _currentBeatWithOffset)
		{
			_nextMeasureBeat++;
		}

		while (_currentNoteIndex < _notes.Length)
		{
			var note = _notes[_currentNoteIndex];
			double spawnBeat = note.Beat - _spawnLeadBeats;

			if (spawnBeat > _currentBeatWithOffset)
			{
				break;
			}

			var activeNote = new ActiveNote
			{
				Index = _currentNoteIndex,
				Note = note,
				State = NoteState.Spawned
			};
			EmitSignal(SignalName.NoteEnter, activeNote.Index, activeNote.Note.Column, spawnBeat, activeNote.Note.Beat);
			_spawnedNotes.Add(activeNote);
			_currentNoteIndex++;
		}

		UpdateNoteStates();
	}

	// TODO: instead of updating every fame, calculate exact distance to "hit" whenever fetched
	private void UpdateNoteStates()
	{
		foreach (var activeNote in _spawnedNotes)
		{

			double hitBeat = activeNote.Note.Beat;
			double beat = _currentBeatWithOffset;

			switch (activeNote.State)
			{
				case NoteState.Spawned:
					if (beat >= hitBeat - _nearWindow)
						activeNote.State = NoteState.EarlyRange;
					break;

				case NoteState.EarlyRange:
					if (beat >= hitBeat - _perfectWindow)
						activeNote.State = NoteState.PerfectRange;
					break;

				case NoteState.PerfectRange:
					if (beat > hitBeat + _perfectWindow)
						activeNote.State = NoteState.LateRange;
					break;

				case NoteState.LateRange:
					if (beat > hitBeat + _nearWindow)
						activeNote.State = NoteState.Missed;
					break;
			}
		}

		_spawnedNotes.Where(activeNote => activeNote.State == NoteState.Missed &&
			_currentBeatWithOffset > activeNote.Note.Beat + _despawnAfter + _nearWindow)
			.ToList()
			.ForEach(activeNote =>
			{
				EmitSignal(SignalName.NoteLeave, activeNote.Index, activeNote.Note.Column);
			});

		_spawnedNotes.RemoveAll(activeNote =>
			activeNote.State == NoteState.Missed &&
			_currentBeatWithOffset > activeNote.Note.Beat + _despawnAfter + _nearWindow);
	}

	public void DestroyActiveNote(int index) {
		var activeNote = _spawnedNotes.FirstOrDefault(n => n.Index == index);
		if (activeNote != null)
		{
			EmitSignal(SignalName.NoteLeave, activeNote.Index, activeNote.Note.Column);
			_spawnedNotes.Remove(activeNote);
		}
	}
}
