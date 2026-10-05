using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node/note_double.svg")]
public partial class RhythmGame : Node
{
	[Export]
	public BeatmapPlayer BeatmapPlayer { get; private set; }

	[Export]
	public RhythmInputController InputController { get; private set; }

	[Export]
	public RhythmUI RhythmUI { get; private set; }

	public void Start()
	{
		BeatmapPlayer.Start("flower_man.json");
	}

	public void Hide()
	{
		RhythmUI.Visible = false;
	}

	public void Show()
	{
		RhythmUI.Visible = true;
	}

}
