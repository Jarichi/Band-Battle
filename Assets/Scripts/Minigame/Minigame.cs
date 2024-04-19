using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    private bool active;
    private PlayerMovement movement;
    private PlayerCombat combat;
    private GameObject weapon;
    private Canvas canvas;

    private void Update()
    {
        if (active) {
            //check if you want to enter combat mode
            if (Input.GetKeyDown(KeyCode.I))
            {
                EngageCombat();
            }
        }
    }

    public void StartMinigame(GameObject player, GameObject weapon)
    {
        canvas = player.GetComponentInChildren<Canvas>();
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
        this.weapon = weapon;

        active = true;
        canvas.enabled= true;
        movement.Disable();
    }

    private void EngageCombat()
    {
        canvas.enabled= false;
        active= false;
        movement.animator.SetTrigger("ObtainGuitar");
        movement.Enable();
        //yield return new WaitForSeconds(1);

        GetComponent<Instrument>().enabled = false;
        combat.Engage(weapon);
    }

}
