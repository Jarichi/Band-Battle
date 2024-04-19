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

    [InspectorButton("EngageCombat")]
    public bool startMinigame;

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
        movement.Enable();

        
        GameObject.Destroy(gameObject);
        combat.Engage(weapon);
    }
}
