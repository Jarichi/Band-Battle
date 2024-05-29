using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerInputController : MonoBehaviour
{
    public float HorizontalMovement { get; private set; }
    public float VerticalMovement { get; private set; }
    public float HorizontalAttack { get; private set; }
    public float VerticalAttack { get; private set; }
    public Action<InputAction.CallbackContext> InteractPressed;
    public Action<InputAction.CallbackContext> EngageCombatPressed;
    public Action<InputAction.CallbackContext> RestartPressed;
    public Action<InputAction.CallbackContext> PlayInput1;
    public Action<InputAction.CallbackContext> PlayInput2;
    public Action<InputAction.CallbackContext> PlayInput3;
    public Action<InputAction.CallbackContext> PlayInput4;
    public static System.EventHandler PlayerJoinEvent;
    public static System.EventHandler PlayerLeaveEvent;
    public static List<PlayerInputController> Players = new();

    public void OnPlayerJoin(PlayerInput input)
    {
        Debug.Log("A player has joined! " + input.gameObject.name);
        Players.Add(this);
        PlayerJoinEvent.Invoke(input.gameObject, new EventArgs());
    }
    public void OnPlayerLeave(PlayerInput input)
    {
        Debug.Log("A player has left! " + input.gameObject.name);
        Players.Remove(this);
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
        context.action.performed += InteractPressed;
    }

    public void OnEngageCombatPressed(InputAction.CallbackContext context)
    {
        context.action.performed += EngageCombatPressed;
    }

    public void OnRestartPressed(InputAction.CallbackContext context)
    {
        context.action.performed += (InputAction.CallbackContext ctx) => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPlay1Pressed(InputAction.CallbackContext context)
    {
        context.action.performed += PlayInput1;
    }

    public void OnPlay2Pressed(InputAction.CallbackContext context)
    {
        context.action.performed += PlayInput2;
    }

    public void OnPlay3Pressed(InputAction.CallbackContext context)
    {
        context.action.performed += PlayInput3;
    }

    public void OnPlay4Pressed(InputAction.CallbackContext context)
    {
        context.action.performed += PlayInput4;
    }

}
