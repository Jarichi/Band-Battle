using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    EventInstance songInstance;
    private FMOD.Studio.EventInstance referenceSongInstance;
    [SerializeField]
    private bool initialized;

    [SerializeField]
    internal InstrumentSoundStatus[] instruments;

    public void Initialize(EventReference song)
    {
        songInstance = FMODUnity.RuntimeManager.CreateInstance(song);
        referenceSongInstance = FMODUnity.RuntimeManager.CreateInstance(song);

        songInstance.start();
        songInstance.release();
        songInstance.setPaused(true);

        referenceSongInstance.setVolume(0f);
        referenceSongInstance.start();
        referenceSongInstance.release();
        referenceSongInstance.setPaused(true);

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
        referenceSongInstance.setPaused(false);
        if (instruments == null)
        {
            instruments = new InstrumentSoundStatus[0];
        }
    }

    public void Stop()
    {
        songInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        referenceSongInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        initialized = false;
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

            float curValue; songInstance.getParameterByName(status.parameterName, out curValue);
            if (curValue != value)
            {
                songInstance.setParameterByName(status.parameterName, value);
                SyncChannels();
            }
            
        }
    }

    void SyncChannels()
    {
        int position1, position2;
        songInstance.getTimelinePosition(out position1);
        referenceSongInstance.getTimelinePosition(out position2);


        if (Mathf.Abs(position1 - position2) > 20)
        {
            Debug.Log("Synced up!");
            songInstance.setTimelinePosition(position2);
            referenceSongInstance.setTimelinePosition(position2);
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
