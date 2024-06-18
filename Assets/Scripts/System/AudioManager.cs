using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    EventInstance songInstance;
    [SerializeField]
    private bool initialized;

    [SerializeField]
    internal InstrumentSoundStatus[] instruments;

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
        if (instruments == null)
        {
            instruments = new InstrumentSoundStatus[0];
        }
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

    public void EnableChannel(string parameterName)
    {
        instruments.ToList().ForEach(i => {
            if (i.parameterName== parameterName)
            {
                i.muted = false;
            }
            }
        );
    }
    public void DisableChannel(string parameterName)
    {
        instruments.ToList().ForEach(i => {
            if (i.parameterName == parameterName)
            {
                i.muted = true;
            }
        }
        );
    }


    private void Update()
    {
        foreach (InstrumentSoundStatus status in instruments)
        {
            var value = 0;
            if (status.muted) { 
                value = 1;
            }

            songInstance.getParameterByName(status.parameterName, out float curValue);
            if (curValue != value)
            {
                songInstance.setParameterByName(status.parameterName, value);
            }
            
        }
    }

    [System.Serializable]
    internal class InstrumentSoundStatus
    {
        [SerializeField]
        internal bool muted;
        [SerializeField]
        internal string parameterName;
    }
}
