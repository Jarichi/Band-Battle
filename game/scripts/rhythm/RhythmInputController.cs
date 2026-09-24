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
					GD.Print($"EARLY!");
					break;
				case NoteState.PerfectRange:
					GD.Print($"PERFECT!");
					break;
				case NoteState.LateRange:
					GD.Print($"LATE!");
					break;
			}

			notesToRemove.Add(note);
		}
		notesToRemove.ForEach(note => _beatmapPlayer.DestroyActiveNote(note.Index));
	}
}
