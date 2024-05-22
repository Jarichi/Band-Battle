using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Minigame : SongHandler
{
    private bool active;
    private PlayerInputController input;
    private PlayerMovement movement;
    private PlayerCombat combat;
    private GameObject weapon;
    private SpriteRenderer ui;
    [SerializeField]
    [Range(0f, 3f)]
    private float transistionAnimationTime;

    private void Start()
    {
        print("start : Minigame");
        input.EngageCombatPressed += TryEngageCombat;
    }

    private void TryEngageCombat(InputAction.CallbackContext obj)
    {
        if (active)
        {
            print("EngageCombat called");
            ui.enabled = false;
            active = false;
            combat.Engage(weapon, movement, GetCombatAnimationName(), transistionAnimationTime);
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
        ui = player.transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.weapon = weapon;

        active = true;
        movement.Disable();
    }

    protected abstract string GetCombatAnimationName();
}
