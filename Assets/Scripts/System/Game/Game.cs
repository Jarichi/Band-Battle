using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private Song selectedSong;
    [SerializeField]
    private Song[] allSongs;    

    [SerializeField]
    private Phase currentPhase;
    [SerializeField]
    Vector2 spawnPoint;
    private new AudioManager audio;
    [SerializeField]
    [Range(0f, 500f)]
    private int timeUntilCombat;
    [SerializeField]
    private SongSelectionScreen songSelectionScreen;
    //private ResultScreen

    public Rhythm Rhythm { get; private set; }
    public static Game Instance;

    void OnEnable()
    {
        Rhythm = GetComponent<Rhythm>();
        audio = GetComponent<AudioManager>();        
        ShowSongs();
        songSelectionScreen.songSelectEvent.AddListener(OnSongSelect);
        Rhythm.rhythmEndEvent.AddListener(End);
    }

    private void OnDisable()
    {
        songSelectionScreen.songSelectEvent.RemoveAllListeners();
        Rhythm.rhythmEndEvent.RemoveListener(End);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    public void DisableAudioChannel(string instrumentId)
    {
        audio.EnableInstrumentEffect(instrumentId, InstrumentEffect.Mute);
    }

    public void EnableAudioChannel(string instrumentId)
    {
        audio.DisableInstrumentEffect(instrumentId, InstrumentEffect.Mute);
    }

    public void EnablePitchShift(string instrumentId)
    {
        audio.EnableInstrumentEffect(instrumentId, InstrumentEffect.Pitchbend);
    }

    public void DisablePitchShift(string instrumentId)
    {
        audio.DisableInstrumentEffect(instrumentId, InstrumentEffect.Pitchbend);
    }

    void ShowSongs()
    {
        currentPhase = Phase.SelectSong;
        songSelectionScreen.ShowUI();
        songSelectionScreen.ShowUI(allSongs);
    }

    public void OnSongSelect(Song song)
    {
        this.selectedSong = song;
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
        Rhythm.StartRhythm(selectedSong, audio, Rhythm.ConsistentTimeEventTriggerRate.QuarterNote);
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