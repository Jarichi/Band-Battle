using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Minigame : SongHandler
{
    private bool active;

    //temp, might not be nessecary
    //private bool started = false;
    
    private PlayerMovement movement;
    private PlayerCombat combat;
    private GameObject weapon;
    private SpriteRenderer ui;
    [SerializeField]
    [Range(0f, 3f)]
    private float startupAnimationLength;

    //gets called when the game starts even when this is inherited form a class
    private void Awake()
    {
        InitRhythmGame();
    }
    private void Update()
    {
        if (active) {
            //check if you want to enter combat mode
            if (Input.GetKeyDown(KeyCode.Return))
            {
                EngageCombat();
            }           

        }
    }

    public void StartMinigame(GameObject player, GameObject weapon)
    {
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
        ui = player.transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.weapon = weapon;

        active = true;
        ui.enabled = true;
        movement.Disable();
    }

    private void EngageCombat()
    {
        ui.enabled= false;
        active = false;
        combat.Engage(weapon, movement, GetCombatAnimationName(), startupAnimationLength);
        GetComponent<Instrument>().enabled = false;
    }

    protected abstract string GetCombatAnimationName();
}
