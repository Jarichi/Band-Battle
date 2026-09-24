using Godot;
using System;

public partial class RhythmUI : Control
{
	private PackedScene _measureScene = GD.Load<PackedScene>("res://scenes/rhythm/ui/measure.tscn");
	private RhythmUIColumn[] _columns;
	[Export]
	private BeatmapPlayer _beatmapPlayer;
	
	[Export(PropertyHint.Range, "10,1500,")]
	private float _pixelsPerBeat = 157.25f;

	private double _currentBeat;
	private int _nextMeasureBeat;
	private bool _hasBeat;

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
		var columnsContainer = GetNode<Control>("HBoxContainer");
		float measureX = columnsContainer.Position.X + _columns[0].Position.X;
		float inputCenterY = columnsContainer.Position.Y
			+ _columns[0].Position.Y
			+ _columns[0].GetInputCenterY();

		if (_hasBeat)
		{
			while (_nextMeasureBeat - _beatmapPlayer.SpawnLeadBeats <= _currentBeat)
			{
				SpawnMeasure(_nextMeasureBeat);
				_nextMeasureBeat++;
			}
		}

		foreach (var child in GetChildren())
		{
			if (child is not Node2D measure || !measure.HasMeta("SpawnBeat"))
			{
				continue;
			}

			double spawnBeat = (double)measure.GetMeta("SpawnBeat");
			if (_currentBeat >= spawnBeat)
			{
				measure.QueueFree();
				continue;
			}

			float lineHalfHeight = measure.GetNode<ColorRect>("ColorRect").Size.Y / 2.0f;
			float pixelsUntilBeat = (float)(spawnBeat - _currentBeat) * _pixelsPerBeat;
			measure.Position = new Vector2(measureX, inputCenterY - lineHalfHeight - pixelsUntilBeat);
		}

		foreach (var column in _columns)
		{
			column.UpdateNotes(_currentBeat, _pixelsPerBeat);
		}
	}

	public void OnBeat(double beatPosition)
	{
		_currentBeat = beatPosition;
		_hasBeat = true;
	}

	public void OnNote(int index, int column, double spawnedOn, double hitBeat)
	{
		if (column < 0 || column >= _columns.Length)
		{
			return;
		}

		_columns[column].SpawnNote(index, spawnedOn, hitBeat);
	}

	public void OnNoteLeave(int index, int column)
	{
		if (column < 0 || column >= _columns.Length)
		{
			return;
		}

		_columns[column].RemoveNote(index);
	}

	private void SpawnMeasure(double beatPosition)
	{
		var measure = _measureScene.Instantiate<Node2D>();
		measure.SetMeta("SpawnBeat", beatPosition);
		measure.Position = new Vector2(0, 0);
		AddChild(measure);
	}
}
