using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    private bool active;
    private PlayerMovement movement;
    private PlayerCombat combat;
    private GameObject weapon;
    private SpriteRenderer ui;
    [SerializeField]
    [Range(0f, 3f)]
    private float startupAnimationLength;

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
