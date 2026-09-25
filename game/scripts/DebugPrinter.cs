using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node/bug.svg")]
public partial class DebugPrinter : Node
{
	public void OnNoteEnter(int index, int column)
	{
		GD.Print($"Note Enter Event: Column {column}, Index {index}");
	}

	public void OnNoteLeave(int index, int column)
	{
		GD.Print($"Note Leave Event: Column {column}, Index {index}");
	}
}
