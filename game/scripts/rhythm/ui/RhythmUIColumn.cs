using Godot;
using System;

public partial class RhythmUIColumn : Panel
{
	[Export]
	private string _inputAction = "";
	[Export]
	private NodePath _inputTexturePath;
	[Export]
	private Texture2D _noteTexture;

	private PackedScene _noteScene = GD.Load<PackedScene>("res://scenes/rhythm/ui/ui_note.tscn");
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

	public float GetInputCenterY()
	{
		return _inputTexture.Position.Y + _inputTexture.Size.Y / 2.0f;
	}

	public void UpdateNotes(double currentBeat, float pixelsPerBeat)
	{
		foreach (var child in GetChildren())
		{
			if (child is not TextureRect note || !note.HasMeta("SpawnBeat"))
			{
				continue;
			}

			double hitBeat = (double)note.GetMeta("HitBeat");
			float beatsUntilHit = (float)(hitBeat - currentBeat);
			float hitY = GetInputCenterY() - note.Size.Y / 2.0f;
			note.Position = new Vector2(note.Position.X, hitY - beatsUntilHit * pixelsPerBeat);
		}
	}

	public void SpawnNote(int index, double spawnedOn, double hitBeat)
	{
		var note = _noteScene.Instantiate<TextureRect>();
		note.SetMeta("Index", index);
		note.SetMeta("SpawnBeat", spawnedOn);
		note.SetMeta("HitBeat", hitBeat);
		note.Position = new Vector2((Size.X - note.Size.X) / 2.0f, 0);
		note.Texture = _noteTexture;
		AddChild(note);
	}

	public void RemoveNote(int index)
	{
		foreach (var child in GetChildren())
		{
			if (child is not TextureRect note || !note.HasMeta("Index"))
			{
				continue;
			}

			int noteIndex = (int)note.GetMeta("Index");
			if (noteIndex == index)
			{
				note.QueueFree();
				break;
			}
		}
	}
}
