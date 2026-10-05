using Godot;
using System;
using System.Collections.Generic;

[GlobalClass, Icon("res://addons/at-icons/node/joypad.svg")]
public partial class RhythmInputController : Node
{
	[Export]
	private string[] _actions = new string[4];
	[Export]
	private BeatmapPlayer _beatmapPlayer;
	[Signal]
	public delegate void NoteHitEventHandler(int index, int column, double score);


	public override void _Process(double delta)
	{
		for (int i = 0; i < _actions.Length; i++)
		{
			if (Input.IsActionJustPressed(_actions[i]))
			{
				HandleInput(i);
			}
		}
	}

	private void HandleInput(int column)
	{
		var notes = _beatmapPlayer.ActiveNotes;
		List<ActiveNote> notesToRemove = new List<ActiveNote>();
		foreach (var note in notes)
		{
			if (note.Note.Column != column || note.State == NoteState.Spawned || note.State == NoteState.Missed)
			{
				continue;
			}

			switch (note.State)
			{
				case NoteState.EarlyRange:
					EmitSignal(SignalName.NoteHit, note.Index, note.Note.Column, 0.5);
					break;
				case NoteState.PerfectRange:
					EmitSignal(SignalName.NoteHit, note.Index, note.Note.Column, 1.0);
					break;
				case NoteState.LateRange:
					EmitSignal(SignalName.NoteHit, note.Index, note.Note.Column, 0.5);
					break;
			}

			notesToRemove.Add(note);
		}
		notesToRemove.ForEach(note => _beatmapPlayer.DestroyActiveNote(note.Index));
	}
}
