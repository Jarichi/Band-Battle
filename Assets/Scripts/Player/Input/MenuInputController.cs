using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputController : MonoBehaviour
{
    public float HorizontalSelect { get; private set; }
    public float VerticalSelect { get; private set; }
    public Action<InputAction.CallbackContext> ConfirmPressed;
    public Action<InputAction.CallbackContext> CancelPressed;
    public Action<InputAction.CallbackContext> StartGamePressed;
    public void OnHorizontalSelect(InputAction.CallbackContext context)
    {
        HorizontalSelect = context.ReadValue<float>();
    }
    public void OnVerticalSelect(InputAction.CallbackContext context)
    {
        VerticalSelect = context.ReadValue<float>();
    }
    public void OnConfirmPressed(InputAction.CallbackContext context)
    {
        if (ConfirmPressed != null)
            context.action.performed += ConfirmPressed;
    }
    public void OnCancelPressed(InputAction.CallbackContext context)
    {
        if (CancelPressed != null)
            context.action.performed += CancelPressed;
    }
    public void OnStartGamePressed(InputAction.CallbackContext context)
    {
        if (StartGamePressed != null)
            context.action.performed += StartGamePressed;
    }
}
