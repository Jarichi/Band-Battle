using Godot;
using System;

public partial class RhythmUI : Control
{


	[Export]
	private string[] _inputActions = new string[4];
	[Export]
	private TextureRect[] _inputTextures = new TextureRect[4];
	[Export]
	private Texture2D[] _noteTextures = new Texture2D[4];
	[Export] 
	private VBoxContainer[] _noteColumns = new VBoxContainer[4];

	private PackedScene _noteScene = GD.Load<PackedScene>("res://scenes/rhythm/ui_note.tscn");
	private PackedScene _measureScene = GD.Load<PackedScene>("res://scenes/rhythm/measure.tscn");


	private double _currentBeat;
	private float _pixelsPerBeat = 157.25f;


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		for (int i = 0; i < _inputActions.Length; i++)
		{
			if (Input.IsActionPressed(_inputActions[i]))
			{
				_inputTextures[i].Modulate = new Color(1, 1, 1, 1);
			}
			else
			{
				_inputTextures[i].Modulate = new Color(1, 1, 1, 0.5f);
			}
		}
		
		foreach (var child in GetChildren())
		{
			if ((child is TextureRect textureRect) && textureRect.HasMeta("SpawnBeat"))
			{
				double spawnBeat = (double)textureRect.GetMeta("SpawnBeat");
				float beatsSinceSpawn = (float)(_currentBeat - spawnBeat);
				textureRect.Position = new Vector2(textureRect.Position.X, beatsSinceSpawn * _pixelsPerBeat - 250 );
			}

			if ((child is Node2D node2D) && node2D.HasMeta("SpawnBeat"))
			{
				double spawnBeat = (double)node2D.GetMeta("SpawnBeat");
				float beatsSinceSpawn = (float)(_currentBeat - spawnBeat);
				node2D.Position = new Vector2(node2D.Position.X, beatsSinceSpawn * _pixelsPerBeat - 250 );
			}
		}
	}

	public void OnBeat(double beatPosition)
	{
		_currentBeat = beatPosition;
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
		float centeredX = _noteColumns[0].GlobalPosition.X - GlobalPosition.X
			+ (_noteColumns[0].Size.X - 60.0f) / 2.0f;
		measure.Position = new Vector2(centeredX, 0);
		AddChild(measure);
	}

	private void SpawnNote(int column, double spawnBeat, double hitBeat)
	{
		var note = _noteScene.Instantiate<TextureRect>();
		note.SetMeta("SpawnBeat", spawnBeat);
		note.SetMeta("HitBeat", hitBeat);
		float centeredX = _noteColumns[column].GlobalPosition.X - GlobalPosition.X
			+ (_noteColumns[column].Size.X - note.Size.X) / 2.0f;
		note.Position = new Vector2(centeredX, 0);
		note.Texture = _noteTextures[column];
		AddChild(note);
	}
}
