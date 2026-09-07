using Godot;
using System;

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
