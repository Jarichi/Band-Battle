using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;

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

    void Start()
    {
        currentPhase = Phase.AwaitInput;
        songs.ToList().ForEach(s => s.DeserializeFile());
    }

    void Update()
    {
        var players = PlayerInputController.Players;
        switch (currentPhase)
        {
            case Phase.AwaitInput:
                if (players.Count() > 0) ShowSongs();
                break;
            case Phase.ChooseInstrument:
                if (players.All(p =>
                {
                    var interaction = p.GetComponent<PlayerWorldInteraction>();
                    Debug.Log(interaction.ChosenInstrument);
                    return interaction.ChosenInstrument != null;
                }))
                {
                    StartPlay();
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

    void StartPlay()
    {
        currentPhase = Phase.Play;
        audioSource.clip = song.audio;
        foreach (var player in PlayerInputController.Players)
        {
            var interaction = player.GetComponent<PlayerWorldInteraction>();
            interaction.ChosenInstrument.Interact(interaction.gameObject);
        }
    }

    public Phase GetCurrentPhase()
    {
        return currentPhase;
    }

    public static Game GetInstance()
    {
        return GameObject.FindAnyObjectByType<Game>();
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