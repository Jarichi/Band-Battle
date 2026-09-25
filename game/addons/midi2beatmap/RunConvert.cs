using Godot;

[Tool]
public partial class RunConvert : EditorScript
{
	public override void _Run()
	{
		var conv = new MIDI2Beatmap();
		conv.Convert("res://assets/midi_legacy/flower_man.mid", "res://addons/midi2beatmap/out/");
	}
}
