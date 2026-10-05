using Godot;
using System;

public partial class BattlePlayer : Node
{
	[Export]
	public int MaxHP { get; private set; } = 150;

	public int CurrentHP { get; private set; }

	public override void _Ready()
	{
		CurrentHP = MaxHP;
	}
} 
