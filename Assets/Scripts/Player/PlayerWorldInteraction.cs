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

    public void Start()
    {
        combat = GetComponent<PlayerCombat>();
        input = GetComponent<PlayerInputController>();
        input.InteractPressed += TryInteract;
    }

    private void Update()
    {
    }

    private void TryInteract(InputAction.CallbackContext ctx)
    {
        if (inRange != null)
        {
            inRange.Interact(gameObject);
            inRange.OnPlaying();
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
