using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node/selection_circle.svg")]
public partial class Spawner : Node
{
	private PackedScene _noteScene = GD.Load<PackedScene>("res://scenes/rhythm/note.tscn");
	private PackedScene _measureScene = GD.Load<PackedScene>("res://scenes/rhythm/measure.tscn");
	[Export]
	private float _pixelsPerBeat = 157.25f;

	private double _currentBeat;

	public void OnBeat(double beatPosition)
	{
		_currentBeat = beatPosition;
	}

	public override void _Process(double delta)
	{
		foreach (var child in GetChildren())
		{
			if (child is Node2D node2D && node2D.HasMeta("SpawnBeat"))
			{
				double spawnBeat = (double)node2D.GetMeta("SpawnBeat");
				float beatsSinceSpawn = (float)(_currentBeat - spawnBeat);
				node2D.Position = new Vector2(node2D.Position.X, beatsSinceSpawn * _pixelsPerBeat);
			}
		}
	}

	public void OnNote(int column, double spawnBeat, double hitBeat)
	{
		SpawnNote(column, spawnBeat, hitBeat);
	}

	public void OnWholeBeat(double spawnBeat)
	{
		SpawnMeasure(spawnBeat);
	}

	private void SpawnMeasure(double beatPosition)
	{
		var measure = _measureScene.Instantiate<Node2D>();
		measure.SetMeta("SpawnBeat", beatPosition);
		measure.Position = Vector2.Zero;
		AddChild(measure);
	}

	private void SpawnNote(int column, double spawnBeat, double hitBeat)
	{
		Color color = column switch
		{
			0 => new Color(1, 0, 0),
			1 => new Color(0, 1, 0),
			2 => new Color(0, 0, 1),
			3 => new Color(1, 1, 0),
			_ => new Color(1, 1, 1)
		};
		var note = _noteScene.Instantiate<Node2D>();
		var panel = note.GetChild<PanelContainer>(0);
		var panelStyle = (StyleBoxFlat)panel.GetThemeStylebox("panel").Duplicate();
		panelStyle.BgColor = color;
		panel.AddThemeStyleboxOverride("panel", panelStyle);
		note.SetMeta("SpawnBeat", spawnBeat);
		note.SetMeta("HitBeat", hitBeat);
		note.Position = new Vector2(column * 60, 0);
		AddChild(note);
	}
}
