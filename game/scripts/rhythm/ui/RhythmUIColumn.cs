using Godot;
using System;

public partial class RhythmUIColumn : PanelContainer
{
	[Export]
	private string _inputAction = "";
	[Export]
	private NodePath _inputTexturePath;
	[Export]
	private Texture2D _noteTexture;

	private PackedScene _noteScene = GD.Load<PackedScene>("res://scenes/rhythm/ui_note.tscn");
	private TextureRect _inputTexture;

	public override void _Ready()
	{
		_inputTexture = GetNode<TextureRect>(_inputTexturePath);
	}

	public override void _Process(double delta)
	{
		_inputTexture.Modulate = Input.IsActionPressed(_inputAction)
			? new Color(1, 1, 1, 1)
			: new Color(1, 1, 1, 0.5f);
	}

	public void UpdateNotes(double currentBeat, float pixelsPerBeat)
	{
		foreach (var child in GetChildren())
		{
			if (child is not TextureRect note || !note.HasMeta("SpawnBeat"))
			{
				continue;
			}

			double spawnBeat = (double)note.GetMeta("SpawnBeat");
			float beatsSinceSpawn = (float)(currentBeat - spawnBeat);
			note.Position = new Vector2(note.Position.X, beatsSinceSpawn * pixelsPerBeat - 250);
		}
	}

	public void SpawnNote(double spawnBeat, double hitBeat)
	{
		var note = _noteScene.Instantiate<TextureRect>();
		note.SetMeta("SpawnBeat", spawnBeat);
		note.SetMeta("HitBeat", hitBeat);
		note.Position = new Vector2((Size.X - note.Size.X) / 2.0f, 0);
		note.Texture = _noteTexture;
		AddChild(note);
	}
}
