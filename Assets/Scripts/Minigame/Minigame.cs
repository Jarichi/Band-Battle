using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Minigame : SongHandler
{

    private double[] song1rhythm = { 1, 2, 3, 4, 4.5f, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15 };

    //TODO: replace with beatmap file

    private NoteDirection[] song1direction =
    {
            NoteDirection.Up,
            NoteDirection.Down,
            NoteDirection.Left,
            NoteDirection.Right,
            NoteDirection.Up,
            NoteDirection.Down,
            NoteDirection.Right,
            NoteDirection.Left,
            NoteDirection.Right,
            NoteDirection.Up,
            NoteDirection.Down,
            NoteDirection.Right,
            NoteDirection.Up,
            NoteDirection.Right,
            NoteDirection.Down,
            NoteDirection.Up
            //NoteDirection.Left

    };



    private bool active;
    private PlayerInputController input;
    private PlayerMovement movement;
    private PlayerCombat combat;
    private GameObject weapon;
    [SerializeField] private string path;

    [SerializeField]
    protected Beatmap beatmap;

    private void Start()
    {
        input.EngageCombatPressed += TryEngageCombat;

        SetNoteSequence(song1rhythm, song1direction);
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

    public void StartMinigame(GameObject player, GameObject weapon)
    {
        input = player.GetComponent<PlayerInputController>();
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
        this.weapon = weapon;

        active = true;
        movement.Disable();
    }
}
