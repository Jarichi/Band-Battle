using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    EventInstance songInstance;
    [SerializeField]
    private bool initialized;

    public void Initialize(EventReference song)
    {
        songInstance = RuntimeManager.CreateInstance(song);

        songInstance.start();
        songInstance.release();
        songInstance.setPaused(true);
        initialized = true;
    }


    public void Play()
    {
        if (!initialized)
        {
            Debug.LogError("Cannot play track since none have been initialized.");
            return;
        }

        songInstance.setPaused(false);
    }

    public void Stop()
    {
        songInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        initialized = false;
    }

    public double GetLength()
    {
        songInstance.getDescription(out var desc);
        desc.getLength(out var length);
        return length / 1000f;
    }

    public void EnableInstrumentEffect(string instrumentId, InstrumentEffect effect)
    {
        SetFMODValue(instrumentId + effect.suffix, 1);
    }

    public void DisableInstrumentEffect(string instrumentId, InstrumentEffect effect)
    {
        SetFMODValue(instrumentId + effect.suffix, 0);
    }

    private void SetFMODValue(string parameter, int value)
    {
        var result = songInstance.getParameterByName(parameter, out var curValue);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError(result);
            return;
        }

        if (curValue == value) return;

        result = songInstance.setParameterByName(parameter, value);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError(result);
            return;
        }
    }

}

[Serializable]
public class InstrumentEffect
{
    public static InstrumentEffect Mute = new("_m");
    public static InstrumentEffect Pitchbend = new("_p");
    internal readonly string suffix;
    internal InstrumentEffect(string suffix)
    {
        this.suffix = suffix;
    }
}
