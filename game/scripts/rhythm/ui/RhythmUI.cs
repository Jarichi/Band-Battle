using Godot;
using System;

public partial class RhythmUI : Control
{
	private PackedScene _measureScene = GD.Load<PackedScene>("res://scenes/rhythm/ui/measure.tscn");
	private RhythmUIColumn[] _columns;
	private double _currentBeat;
	private float _pixelsPerBeat = 157.25f;

	public override void _Ready()
	{
		_columns = new[]
		{
			GetNode<RhythmUIColumn>("HBoxContainer/Column1"),
			GetNode<RhythmUIColumn>("HBoxContainer/Column2"),
			GetNode<RhythmUIColumn>("HBoxContainer/Column3"),
			GetNode<RhythmUIColumn>("HBoxContainer/Column4")
		};
	}

	public override void _Process(double delta)
	{
		foreach (var child in GetChildren())
		{
			if (child is not Node2D measure || !measure.HasMeta("SpawnBeat"))
			{
				continue;
			}

			double spawnBeat = (double)measure.GetMeta("SpawnBeat");
			float beatsSinceSpawn = (float)(_currentBeat - spawnBeat);
			measure.Position = new Vector2(measure.Position.X, beatsSinceSpawn * _pixelsPerBeat - 250);
		}

		foreach (var column in _columns)
		{
			column.UpdateNotes(_currentBeat, _pixelsPerBeat);
		}
	}

	public void OnBeat(double beatPosition)
	{
		_currentBeat = beatPosition;
	}

	public void OnNote(int column, double spawnBeat, double hitBeat)
	{
		if (column < 0 || column >= _columns.Length)
		{
			return;
		}

		_columns[column].SpawnNote(spawnBeat, hitBeat);
	}

	public void OnWholeBeat(double spawnBeat)
	{
		SpawnMeasure(spawnBeat);
	}

	private void SpawnMeasure(double beatPosition)
	{
		var measure = _measureScene.Instantiate<Node2D>();
		measure.SetMeta("SpawnBeat", beatPosition);
		measure.Position = new Vector2(0, 0);
		AddChild(measure);
	}
}
