using Godot;
using System;

public partial class RhythmRow : Node
{
	[Export]
	private ColorRect _inputRect;
	private Color _inputRectInitColor;
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
	}
}
