using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerWorldInteraction : MonoBehaviour
{
    private double score;
    private PlayerCombat combat;
    private PlayerInput input;
    private PlayerMovement movement;
    private Instrument inRange;
    public Instrument ChosenInstrument {get; private set;}
    public void Start()
    {
        input = Player.OfEntity(gameObject).Input;
        movement = GetComponent<PlayerMovement>();
        ChosenInstrument = null;
        input.actions["Overworld Interact"].performed += TryInteract;
    }

    private void OnDisable()
    {
        input.actions["Overworld Interact"].performed -= TryInteract;
    }

    private void TryInteract(InputAction.CallbackContext ctx)
    {
        if (Game.Instance.GetCurrentPhase() != Game.Phase.ChooseInstrument)
        return;

        if (inRange != null)
        {
            ChosenInstrument = inRange;
            movement.Disable();
            if (PlayerList.Get().All(player =>
            {
                var interaction = player.InGameEntity.GetComponent<PlayerWorldInteraction>();
                return interaction.ChosenInstrument != null;
            }))
            {
                Game.Instance.StartPlayPhase();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Instrument>(out var inRangeOfInstrument))
        {
            inRange = inRangeOfInstrument;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        inRange = null;
    }

    public void AddScore(double score)
    {
        this.score += score;
    }

    public void DecreaseScore(double score) {
        this.score -= score;
    }

    public double GetScore()
    {
        return score;
    }
}
