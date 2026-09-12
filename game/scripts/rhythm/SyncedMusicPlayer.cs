using Godot;
using System;

public partial class SyncedMusicPlayer : AudioStreamPlayer2D
{
	[Export]
	private float _beatOffset = 4f;

	[Export]
	private Timer _timer;

	private float _bpm;
	private float _secondsPerBeat => 60f / _bpm;

	private double _songPosition = 0f;
	private double _songPositionInBeats = 0;
	private int _lastIntegerBeat = int.MinValue;
	private int _preRollBeat;

	public float BeatOffset => _beatOffset;

	[Signal]
	public delegate void BeatEventHandler(double beatPosition);

	[Signal]
	public delegate void IntegerBeatEventHandler(int beatPosition);

	public void Start(float bpm)
	{
		_bpm = bpm;
		_timer.WaitTime = _secondsPerBeat;
		_timer.Timeout += () =>
		{
			_preRollBeat++;
			if (_preRollBeat < _beatOffset - 1)
			{
				_timer.Start();
			}
			else if (_preRollBeat == _beatOffset - 1)
			{
				_timer.WaitTime = _timer.WaitTime - (AudioServer.GetTimeToNextMix() + AudioServer.GetOutputLatency());
				_timer.Start();
			}
			else
			{
				Play();
				_timer.Stop();
			}
		};
		_timer.Start();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsPlaying())
		{
			_songPosition += delta;
			_songPositionInBeats = _songPosition / _secondsPerBeat;
			EmitBeat();
			return;
		}
		_songPosition = GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
		_songPosition -= AudioServer.GetOutputLatency();
		_songPositionInBeats = _songPosition / _secondsPerBeat + _beatOffset;
		EmitBeat();
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
