using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Burst;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    private Song song;
    [SerializeField]
    private Phase currentPhase;
    private new AudioManager audio;
    private SongSelection songSelection;
    public static Game Instance;

    void Start()
    {
        audio = GetComponent<AudioManager>();
        currentPhase = Phase.SelectSong;
        
        ShowSongs();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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
        currentPhase = Phase.SelectSong;
        /*song = songSelection
        PlayerList.Get().ToList().ForEach(p => {
            p.Spawn(Vector2.zero);
            p.SwitchActionMap(Player.PlayActionMap);
        }
        );
        currentPhase = Phase.ChooseInstrument;
        audio.Initialize(song.fmodEvent);*/
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
        PlayerList.Get().ForEach(player =>
        {
            player.Despawn();
            player.SwitchActionMap(Player.MenuActionMap);
        });
    }

    public Phase GetCurrentPhase()
    {
        return currentPhase;
    }
}