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
    private AudioSource audioSource;
    public static Game Instance;
    private PlayerInputManager inputManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
                if (playerCount > 1)
                {
                    ShowSongs();
                    inputManager.DisableJoining();
                }
                break;
            case Phase.Play:
                if (playerCount == 1) {
                    End();
                }
                break;
        }
    }

    void ShowSongs()
    {
        currentPhase = Phase.SelectSong;
        song = songs[0];
        currentPhase = Phase.ChooseInstrument;
    }

    public void StartPlayPhase()
    {
        currentPhase = Phase.Play;
        audioSource.clip = song.audio;
        foreach (var player in PlayerInputController.GetPlayers())
        {
            var interaction = player.GetComponent<PlayerWorldInteraction>();
            interaction.ChosenInstrument.StartMinigame(interaction.gameObject);
        }
        audioSource.Play();
    }

    public void End()
    {
        currentPhase = Phase.End;
        audioSource.Stop();
        PlayerInputController.GetPlayers().ForEach(obj =>
        {
            obj.GetComponent<PlayerMovement>().Disable();
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
    public AudioClip audio;
    public Beatmap beatmap;
    internal void DeserializeFile()
    {
        beatmap = JsonUtility.FromJson<Beatmap>(File.ReadAllText(beatmapFilePath));
    }
}