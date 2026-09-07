using Godot;
using System;

public partial class RhythmColumn : Node
{
	[Export]
	private ColorRect _inputRect;
	private Color _inputRectInitColor;
    [Export]
	private Area2D _hitArea;
	[Export]
	private ColorRect _bgRect;
	[Export]
	private String _inputAction;

	public override void _Ready()
	{
		_inputRectInitColor = _inputRect.Color;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed(_inputAction))
		{
			_inputRect.Color = new Color(1, 1, 1, 1);
		}
		else
		{
			var returnT = Mathf.Clamp((float)delta * 8.0f, 0.0f, 1.0f);
			_inputRect.Color = _inputRect.Color.Lerp(_inputRectInitColor, returnT);
		}

		if (Input.IsActionJustPressed(_inputAction))
		{
			TryHitNote();
		}
	}

	private void TryHitNote()
	{
		foreach (var note in _hitArea.GetOverlappingAreas())
		{
			switch (note.Name)
			{
				case "PerfectBox":
					GD.Print("Perfect!");
					note.GetParent().QueueFree();
					return;
				case "EarlyBox":
					GD.Print("Early!");
					note.GetParent().QueueFree();
					return;
				case "LateBox":
					GD.Print("Late!");
					note.GetParent().QueueFree();
					return;
			}
		}
	}
}
