using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using static Rhythm;
using static TriggerLine;

public class Minigame : MonoBehaviour
{
    public enum MistakeType
    {
        WrongNote,
        MissedNote
    }

    private PlayerMovement movement;
    private PlayerCombat combat;
    private PlayerRhythm playerRhythm;
    private PlayerInput input;
    private GameObject weapon;
    [SerializeField]
    private GameObject notePrefab;
    private Game game;
    private Note noteController;

    private double scoreIncrease;
    private TriggerLine triggerLine;

    private void Start()
    {
        input = Player.OfEntity(gameObject).Input;
        playerRhythm = GetComponentInParent<PlayerRhythm>();
        input.actions["Engage Combat"].performed += TryEngageCombat;
        game = Game.Instance;
        var rhythm = game.Rhythm;
        rhythm.noteSpawnEvent.AddListener(SpawnNote);
        rhythm.consistentTimeEvent.AddListener(SpawnPulse);

        scoreIncrease = game.Rhythm.CalculateScoreIncreasePerNote(playerRhythm.ChosenInstrument.id);
        triggerLine = GetComponentInChildren<TriggerLine>();
        triggerLine.NoteCollideEvent.AddListener(OnHit);
        triggerLine.NotePassEvent.AddListener((note, activeColliders)
            => OnMiss(activeColliders ? MistakeType.WrongNote : MistakeType.MissedNote, note));
        triggerLine.NoCollisionEvent.AddListener(() => OnMiss(MistakeType.WrongNote));

        noteController = notePrefab.GetComponent<Note>();
    }

    private void OnDisable()
    {
        input.actions["Engage Combat"].performed -= TryEngageCombat;
        var rhythm = Game.Instance.Rhythm;
        rhythm.noteSpawnEvent.RemoveListener(SpawnNote);
        rhythm.consistentTimeEvent.RemoveListener(SpawnPulse);
    }

    private void TryEngageCombat(InputAction.CallbackContext obj)
    {
        if (Game.Instance.Rhythm.Active && combat.allowCombat)
        {
            combat.Engage(weapon, movement);
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    void SpawnNote(NoteDirection direction, string instrumentId)
    {
        if (playerRhythm.ChosenInstrument.id != instrumentId)
            return;


        float xOffset = 0;
        switch (direction)
        {
            case NoteDirection.Left:
                noteController.SetColor(Color.red);
                xOffset = -1.5f;
                break;
            case NoteDirection.Right:
                noteController.SetColor(Color.blue);
                xOffset = -0.5f;
                break;
            case NoteDirection.Up:
                noteController.SetColor(Color.yellow);
                xOffset = 0.5f;
                break;
            case NoteDirection.Down:
                noteController.SetColor(Color.green);
                xOffset = 1.5f;
                break;

            default:
                Debug.LogError("Load a beatmap first before spawning beatmapNotes.");
                break;
        }

        Vector3 spawnPos = transform.position + new Vector3(xOffset, 0f, 0f);
        notePrefab.GetComponent<Note>().SetDirection(direction);
        Instantiate(notePrefab, spawnPos, Quaternion.identity, this.transform);
    }

    void SpawnPulse(double _)
    {
        var pulsemarker = Resources.Load<GameObject>("Prefabs/Minigame/Pulse Marker");
        Instantiate(pulsemarker, this.transform);
    }

    public void OnRhythmStart(Player player, GameObject weapon)
    {
        movement = player.InGameEntity.GetComponent<PlayerMovement>();
        combat = player.InGameEntity.GetComponent<PlayerCombat>();
        this.weapon = weapon;

        movement.Disable();
    }

    public void OnMiss(MistakeType reason, Note note = null)
    {
        if (reason == MistakeType.WrongNote)
        {
            game.EnablePitchShift(playerRhythm.ChosenInstrument.id);
        }
        else if (reason == MistakeType.MissedNote)
        {
            game.DisableAudioChannel(playerRhythm.ChosenInstrument.id);
        }
    }

    public void OnHit(Note note)
    {
        playerRhythm.AddScore(scoreIncrease);
        game.EnableAudioChannel(playerRhythm.ChosenInstrument.id);
        game.DisablePitchShift(playerRhythm.ChosenInstrument.id);
    }
}
