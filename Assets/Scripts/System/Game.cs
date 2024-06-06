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
        AwaitInput,
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
    private AudioManager audio;
    public static Game Instance;
    private PlayerInputManager inputManager;
    public int minPlayerCount;
    private int killedPlayers = 0;


    void Start()
    {
        audio = GetComponent<AudioManager>();
        currentPhase = Phase.AwaitInput;
        songs.ToList().ForEach(s => s.DeserializeFile());
        Instance = GameObject.FindAnyObjectByType<Game>();
        inputManager = GetComponent<PlayerInputManager>();
        inputManager.EnableJoining();
    }

    void Update()
    {
        var playerCount = PlayerInputController.PlayerCount();
        switch (currentPhase)
        {
            case Phase.AwaitInput:
                if (playerCount > minPlayerCount)
                {
                    ShowSongs();
                    inputManager.DisableJoining();
                }
                break;
        }
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
        gui.ToggleStartScreen();
        PlayerInputController.GetPlayers().ForEach(p => p.GetComponent<PlayerMovement>().Enable());
        currentPhase = Phase.SelectSong;
        song = songs[0];
        currentPhase = Phase.ChooseInstrument;
        audio.Initialize(song.fmodEvent);
    }

    public void StartPlayPhase()
    {
        currentPhase = Phase.Play;
        foreach (var player in PlayerInputController.GetPlayers())
        {
            var interaction = player.GetComponent<PlayerWorldInteraction>();
            interaction.ChosenInstrument.StartMinigame(interaction.gameObject, song.beatmap);
        }
        audio.Play();
    }

    public void End()
    {
        currentPhase = Phase.End;
        audio.Stop();
        PlayerInputController.GetPlayers().ForEach(obj =>
        {
            obj.GetComponent<PlayerMovement>().Disable();
            Debug.Log(obj.GetComponent<PlayerWorldInteraction>().GetScore());
        });
    }

    public Phase GetCurrentPhase()
    {
        return currentPhase;
    }

    public void OnPlayerDeath()
    {
        killedPlayers++;
        if (currentPhase == Phase.Play && killedPlayers == PlayerInputController.PlayerCount() - 1)
        {
            End();
        }
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