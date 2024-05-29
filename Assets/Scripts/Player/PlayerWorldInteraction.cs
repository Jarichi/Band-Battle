using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerWorldInteraction : MonoBehaviour
{
    private PlayerCombat combat;
    private PlayerInputController input;
    private Instrument inRange;
    public Instrument ChosenInstrument {get; private set;}
    public void Start()
    {
        combat = GetComponent<PlayerCombat>();
        input = GetComponent<PlayerInputController>();
        ChosenInstrument = null;
        input.InteractPressed += TryInteract;
    }
    
    private void TryInteract(InputAction.CallbackContext ctx)
    {
        if (Game.GetInstance().GetCurrentPhase() != Game.Phase.ChooseInstrument)
        return;

        if (inRange != null)
        {
            ChosenInstrument = inRange;
            Debug.Log("interacted: " + ChosenInstrument);
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
}
