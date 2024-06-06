using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minigame : SongHandler
{
    private bool active;
    private PlayerInputController input;
    private PlayerMovement movement;
    private PlayerCombat combat;
    private GameObject weapon;
    [SerializeField] private string path;
    [SerializeField] private Sprite GUISprite;


    private void Start()
    {
        input.EngageCombatPressed += TryEngageCombat;

        
        InitRhythmGame();
    }

    private void TryEngageCombat(InputAction.CallbackContext obj)
    {
        if (active)
        {
            active = false;
            combat.Engage(weapon, movement);
            Destroy(gameObject); gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (active) {
            RunRhythmGame();
        }
    }

    public void StartMinigame(GameObject player, GameObject weapon, Beatmap beatmap, int channelNumber)
    {
        bpm = beatmap.BPM;
        Channel channel = beatmap.Channels.Find(ch => ch.Number == channelNumber);
        var beats = channel.Positions.ConvertAll(position => position.Beat);
        var directions = channel.Positions.ConvertAll(position => (NoteDirection)position.Direction);
        SetNoteSequence(beats.ToArray(), directions.ToArray());

        input = player.GetComponent<PlayerInputController>();
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
        this.weapon = weapon;

        active = true;
        movement.Disable();
    }

}
