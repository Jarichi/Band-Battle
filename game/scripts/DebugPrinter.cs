using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node/bug.svg")]
public partial class DebugPrinter : Node
{
	public void OnBeat(double pos)
	{
		GD.Print($"Beat: {pos}");
	}

	public void OnIntegerBeat(int pos)
	{
		GD.Print($"Integer Beat: {pos}");
	}
}
