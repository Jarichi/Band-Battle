using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node/sword.svg")]
public partial class BattleSequence : Node
{
	[Export]
	public RhythmGame RhythmGame { get; private set; }

	[Export]
	public BattlePlayer Player { get; private set; }

	[Export]
	public BattlePlayer OpponentPlayer { get; private set; }

	private bool _isPlayerTurn = true;
	private BattlePhase _currentPhase = BattlePhase.Music;
	private double _phaseScore = 0.0;

	public override void _Ready()
	{
		RhythmGame.Start();
		RhythmGame.InputController.NoteHit += OnNoteHit;
		RhythmGame.BeatmapPlayer.SectionStart += OnSectionStart;
		RhythmGame.BeatmapPlayer.SectionEnd += OnSectionEnd;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		RhythmGame.InputController.NoteHit -= OnNoteHit;
	}


	private void OnNoteHit(int index, int column, double score)
	{
		_phaseScore += score;
		GD.Print(_phaseScore);
	}

	private void OnSectionStart()
	{
		//RhythmGame.Show();
	}

	private void OnSectionEnd()
	{
		// RhythmGame.Hide();
	}

	private void SwapTurn()
	{
		_isPlayerTurn = !_isPlayerTurn;
		_currentPhase = BattlePhase.Music;
		_phaseScore = 0.0;
	}

	private double CalculateDamage(int sectionNoteCount, int fakeNoteCount)
	{
		double baseDamage = 1f;
		double accuracy = _phaseScore / (sectionNoteCount + fakeNoteCount);
		return baseDamage * accuracy;
	}

	private enum BattlePhase
	{
		Music,
		Attack,
	}
}
