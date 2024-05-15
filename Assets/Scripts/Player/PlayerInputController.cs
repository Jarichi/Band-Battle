using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public float HorizontalMovement { get; private set; }
    public float VerticalMovement { get; private set; }
    public float HorizontalAttack { get; private set; }
    public float VerticalAttack { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool EngageCombatPressed { get; private set; }
    public bool RestartPressed { get; private set; }
    public static System.EventHandler PlayerJoinEvent;
    public static System.EventHandler PlayerLeaveEvent;

    public void OnPlayerJoin(PlayerInput input)
    {
        Debug.Log("A player has joined! " + input.gameObject.name);
        PlayerJoinEvent.Invoke(input.gameObject, new EventArgs());
    }
    public void OnPlayerLeave(PlayerInput input)
    {
        Debug.Log("A player has left! " + input.gameObject.name);
        PlayerLeaveEvent.Invoke(input.gameObject, new EventArgs());
    }

    public void OnHorizontalMove(InputAction.CallbackContext context)
    {
        HorizontalMovement = context.ReadValue<float>();
    }

    public void OnVerticalMove(InputAction.CallbackContext context)
    {
        VerticalMovement = context.ReadValue<float>();
    }

    public void OnHorizontalAttack(InputAction.CallbackContext context)
    {
        HorizontalAttack = context.ReadValue<float>();
    }

    public void OnVerticalAttack(InputAction.CallbackContext context)
    {
        VerticalAttack = context.ReadValue<float>();
    }

    public void OnInteractionPressed(InputAction.CallbackContext context)
    {
        InteractPressed = context.action.triggered;
    }

    public void OnEngageCombatPressed(InputAction.CallbackContext context)
    {
        EngageCombatPressed = context.action.triggered;
    }

    public void OnRestartPressed(InputAction.CallbackContext context)
    {
        RestartPressed = context.action.triggered;
    }
}
