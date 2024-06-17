using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Rhythm;

public class Minigame : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerCombat combat;
    private PlayerRhythm playerRhythm;
    private PlayerInput input;
    private GameObject weapon;
    [SerializeField]
    private GameObject notePrefab;
    [SerializeField]
    private Sprite[] noteColours;

    private void Start()
    {
        input = Player.OfEntity(gameObject).Input;
        playerRhythm = GetComponentInParent<PlayerRhythm>();
        input.actions["Engage Combat"].performed += TryEngageCombat;
        Game.Instance.Rhythm.noteSpawnEvent.AddListener(SpawnNote);
    }

    private void OnDisable()
    {
        input.actions["Engage Combat"].performed -= TryEngageCombat;
        Game.Instance.Rhythm.noteSpawnEvent.RemoveListener(SpawnNote);
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

    void SpawnNote(NoteDirection direction, int channel)
    {
        if (!playerRhythm.ChosenInstrument.IsCorrectChannel(channel))
            return;

        float xOffset = 0;
        Vector3 spawnPos = Vector3.zero;
        switch (direction)
        {
            case NoteDirection.Left:
                notePrefab.GetComponent<SpriteRenderer>().sprite = noteColours[0];
                //red note
                xOffset = -1.5f;
                break;
            case NoteDirection.Right:
                //blue note
                notePrefab.GetComponent<SpriteRenderer>().sprite = noteColours[1];
                xOffset = -0.5f;
                break;
            case NoteDirection.Up:
                //yellow note
                notePrefab.GetComponent<SpriteRenderer>().sprite = noteColours[2];
                xOffset = 0.5f;
                break;
            case NoteDirection.Down:
                //green note
                notePrefab.gameObject.GetComponent<SpriteRenderer>().sprite = noteColours[3];
                xOffset = 1.5f;
                break;

            default:
                //error handler
                Debug.Log("Load a beatmap first before spawning beatmapNotes.");
                break;
        }

        spawnPos = transform.position + new Vector3(xOffset, 0f, 0f);
        notePrefab.GetComponent<NoteController>().SetDirection(direction);
        Instantiate(notePrefab, spawnPos, Quaternion.identity, this.transform);
    }

    public void OnRhythmStart(Player player, GameObject weapon)
    {
        movement = player.InGameEntity.GetComponent<PlayerMovement>();
        combat = player.InGameEntity.GetComponent<PlayerCombat>();
        this.weapon = weapon;

        movement.Disable();
    }

}
