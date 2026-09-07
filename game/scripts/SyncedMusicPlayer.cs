using Godot;
using System;

public partial class SyncedMusicPlayer : AudioStreamPlayer2D
{
	[Export]
	private float _bpm = 160;
	[Export]
	private float _beatOffset = 0;

	private float _secondsPerBeat => 60f / _bpm;

	private double _songPosition = 0f;
	private double _songPositionInBeats = 0;
	private int _lastIntegerBeat = int.MinValue;

	[Signal]
	public delegate void BeatEventHandler(double beatPosition);
	
	[Signal]
	public delegate void IntegerBeatEventHandler(int beatPosition);

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Playing)
		{
			_songPosition = GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
			_songPosition -= AudioServer.GetOutputLatency();
			_songPositionInBeats = _songPosition / _secondsPerBeat + _beatOffset;
			EmitBeat();
		}
	}

	private void EmitBeat()
	{
		int currentIntegerBeat = (int)Math.Floor(_songPositionInBeats);
		if (currentIntegerBeat != _lastIntegerBeat)
		{
			_lastIntegerBeat = currentIntegerBeat;
			EmitSignal(SignalName.IntegerBeat, currentIntegerBeat);
		}

		EmitSignal(SignalName.Beat, _songPositionInBeats);
	}

	public double GetSongPosition()
	{
		return _songPosition;
	}

}
