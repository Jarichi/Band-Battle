using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public EventReference song;
    EventInstance songInstance;

    [SerializeField]
    internal InstrumentSoundStatus[] instruments;

    void Start()
    {
        songInstance = FMODUnity.RuntimeManager.CreateInstance(song);
        songInstance.start();
        songInstance.release();

        if (instruments == null)
        {
            instruments = new InstrumentSoundStatus[0];
        }
    }

    private void Update()
    {
        foreach (InstrumentSoundStatus status in instruments)
        {
            var value = 0;
            if (status.muted) value = 1;
            songInstance.setParameterByName(status.parameterName, value);
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
