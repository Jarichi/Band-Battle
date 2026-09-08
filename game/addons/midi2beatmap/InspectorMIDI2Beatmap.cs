using Godot;
using System;

[Tool]
public partial class InspectorMIDI2Beatmap : EditorInspectorPlugin
{
	private MIDI2Beatmap _parent;

	public InspectorMIDI2Beatmap(MIDI2Beatmap parent)
	{
		_parent = parent;
	}

	public override bool _CanHandle(GodotObject obj)
	{
		if (obj is Script script)
		{
			var path = script.ResourcePath;
			if (string.IsNullOrEmpty(path))
				return false;
			return path.EndsWith("MIDI2Beatmap.cs") || path.EndsWith("RunConvert.cs");
		}
		return false;
	}

	public override void _ParseBegin(GodotObject obj)
	{
		var v = new VBoxContainer();

		var midiLabel = new Label();
		midiLabel.Text = "MIDI Path (res://)";
		var midiEdit = new LineEdit();
		midiEdit.Text = "res://assets/midi_legacy/flower_man.mid";

		var outLabel = new Label();
		outLabel.Text = "Output Path (res://)";
		var outEdit = new LineEdit();
		outEdit.Text = "res://addons/midi2beatmap/out/";

		var btn = new Button();
		btn.Text = "Convert";
		btn.Pressed += () =>
		{
			if (_parent != null)
			{
				_parent.Convert(midiEdit.Text, outEdit.Text);
			}
		};

		v.AddChild(midiLabel);
		v.AddChild(midiEdit);
		v.AddChild(outLabel);
		v.AddChild(outEdit);
		v.AddChild(btn);

		AddCustomControl(v);
	}
}
