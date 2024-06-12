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
using UnityEngine.SceneManagement;
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
    [SerializeField]
    Vector2 spawnPoint;
    private new AudioManager audio;
    [SerializeField]
    [Range(0f, 500f)]
    private int timeUntilCombat;
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
        currentPhase = Phase.SelectSong;
    }

    public void OnSongSelect(Song song)
    {
        this.song = song;
        var gui = UserInterface.Instance;
        gui.GetComponentInChildren<SongSelectionScreen>().gameObject.SetActive(false);
        currentPhase = Phase.ChooseInstrument;
        audio.Initialize(song.fmodEvent);
        PlayerList.Get().ForEach(p =>
        {
            p.Spawn(spawnPoint);
        });
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
        StartCoroutine(EnableCombat());
    }

    private IEnumerator EnableCombat()
    {
        yield return new WaitForSeconds(timeUntilCombat);
        PlayerList.Get().ForEach(p => { p.InGameEntity.GetComponent<PlayerCombat>().allowCombat = true; });
    }

    public void End()
    {
        currentPhase = Phase.End;
        audio.Stop();
        PlayerList.Get().ForEach(player =>
        {
            player.Despawn();
        });
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public Phase GetCurrentPhase()
    {
        return currentPhase;
    }
}