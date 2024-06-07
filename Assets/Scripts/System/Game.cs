using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    [Serializable]
    public enum Phase
    {
        SelectSong,
        ChooseInstrument,
        Play,
        End
    }
    [SerializeField]
    private Song[] songs;
    private Song song;
    [SerializeField]
    private Phase currentPhase;
    private new AudioManager audio;
    public static Game Instance;


    void Start()
    {
        audio = GetComponent<AudioManager>();
        currentPhase = Phase.SelectSong;
        songs.ToList().ForEach(s => s.DeserializeFile());
        Instance = GameObject.FindAnyObjectByType<Game>();
        ShowSongs();

    }

    void Update()
    {
    }

    public void DisableAudioChannel(string parameterName)
    {
        audio.DisableChannel(parameterName);
    }

    public void EnableAudioChannel(string parameterName)
    {
        audio.EnableChannel(parameterName);
    }

    void ShowSongs()
    {
        var gui = UserInterface.Instance;
        //gui.ToggleStartScreen();
        currentPhase = Phase.SelectSong;
        song = songs[0];
        PlayerList.Get().ToList().ForEach(p => p.Spawn(Vector2.zero));
        currentPhase = Phase.ChooseInstrument;
        audio.Initialize(song.fmodEvent);
    }

    public void StartPlayPhase()
    {
        currentPhase = Phase.Play;
        foreach (var player in PlayerList.Get())
        {
            var interaction = player.InGameEntity.GetComponent<PlayerWorldInteraction>();
            interaction.ChosenInstrument.StartMinigame(interaction.gameObject, song.beatmap);
        }
        audio.Play();
    }

    public void End()
    {
        currentPhase = Phase.End;
        audio.Stop();
        PlayerList.Get().ForEach(obj =>
        {
            obj.InGameEntity.GetComponent<PlayerMovement>().Disable();
        });
    }

    public Phase GetCurrentPhase()
    {
        return currentPhase;
    }
}

[Serializable]
public class Song
{
    [SerializeField]
    private string beatmapFilePath;
    public EventReference fmodEvent;
    public Beatmap beatmap;
    internal void DeserializeFile()
    {
        beatmap = JsonUtility.FromJson<Beatmap>(File.ReadAllText(beatmapFilePath));
    }
}