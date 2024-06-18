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

    private NoteController noteController;

    private void Start()
    {
        input = Player.OfEntity(gameObject).Input;
        playerRhythm = GetComponentInParent<PlayerRhythm>();
        input.actions["Engage Combat"].performed += TryEngageCombat;
        var rhythm = Game.Instance.Rhythm;
        rhythm.noteSpawnEvent.AddListener(SpawnNote);
        rhythm.consistentTimeEvent.AddListener(SpawnPulse);

        noteController = notePrefab.GetComponent<NoteController>();
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
                //red note
                xOffset = -1.5f;
                break;
            case NoteDirection.Right:
                //blue note
                noteController.SetColor(Color.blue);
                xOffset = -0.5f;
                break;
            case NoteDirection.Up:
                //yellow note
                noteController.SetColor(Color.green);
                xOffset = 0.5f;
                break;
            case NoteDirection.Down:
                //green note
                noteController.SetColor(Color.yellow);
                xOffset = 1.5f;
                break;

            default:
                //error handler
                Debug.Log("Load a beatmap first before spawning beatmapNotes.");
                break;
        }

        Vector3 spawnPos = transform.position + new Vector3(xOffset, 0f, 0f);
        notePrefab.GetComponent<NoteController>().SetDirection(direction);
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

}
