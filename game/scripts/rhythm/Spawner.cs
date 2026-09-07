using Godot;
using System;
using System.Collections.Generic;

public partial class Spawner : Node
{
	private PackedScene _measureScene = GD.Load<PackedScene>("res://scenes/rhythm/Measure.tscn");
	[Export]
	private float _pixelsPerBeat = 157.25f;

	private readonly Dictionary<Node2D, double> _spawnBeats = new();
	private double _currentBeat;

	public override void _Process(double delta)
	{
		foreach (var child in GetChildren())
		{
			if (child is Node2D node2D)
			{
				if (_spawnBeats.TryGetValue(node2D, out double spawnBeat))
				{
					float beatsSinceSpawn = (float)(_currentBeat - spawnBeat);
					node2D.Position = new Vector2(-beatsSinceSpawn * _pixelsPerBeat, node2D.Position.Y);
				}
			}
		}
	}

	public void OnBeat(double beatPosition)
	{
		_currentBeat = beatPosition;
	}

	public void OnIntegerBeat(int beatPosition)
	{
		if (beatPosition % 2 == 0)
		{
			SpawnMeasure(beatPosition);
		}
	}

	private void SpawnMeasure(int beatPosition)
	{
		var measure = _measureScene.Instantiate<Node2D>();
		_spawnBeats[measure] = beatPosition;
		AddChild(measure);
	}
}
